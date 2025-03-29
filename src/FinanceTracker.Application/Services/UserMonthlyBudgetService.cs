using FinanceTracker.Application.Utils;
using FinanceTracker.Domain.Common.Pagination;
using FinanceTracker.Domain.Dtos.UserMonthlyBudgets;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Interfaces;
using FinanceTracker.Infrastructure.Context;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Application.Services;

public class UserMonthlyBudgetService : IUserMonthlyBudgetService
{
    private readonly FinanceTrackerDbContext _dbContext;
    
    public UserMonthlyBudgetService(FinanceTrackerDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Result> GenerateMonthlyBudgetAsync(Guid userId, CancellationToken ct)
    {
        try
        {
            var beginningOfMonth = ToBeginningOfMonth(DateTime.UtcNow);
            
            var budgetAlreadyExist = _dbContext.UserMonthlyBudgets
                .Any(b => b.UserId == userId 
                    && b.Date == beginningOfMonth);

            if (budgetAlreadyExist)
            {
                return Result.Fail("UserMonthlyBudget already exist");
            }
            
            var userMonthlyBudgetAmount = await _dbContext.Incomes
                .Where(i => i.UserId == userId
                            && i.IsActiveThisMonth ==  true)
                .SumAsync(i => i.Amount, ct);

            var budget = new UserMonthlyBudget
            {
                UserId = userId,
                Date = ToBeginningOfMonth(DateTime.UtcNow),
                TotalBudget = userMonthlyBudgetAmount,
            };
        
            await _dbContext.UserMonthlyBudgets.AddAsync(budget, ct);
            await _dbContext.SaveChangesAsync(ct);
    
            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail($"Error generating budget: {ex.Message}");
        }
    }

    public async Task<Result> EnsureCreatedAsync(CancellationToken ct)
    {
        var beginningOfMonth = ToBeginningOfMonth(DateTime.UtcNow);
        
        var usersWithOutBudget = await _dbContext.Users
            .Where(u => !_dbContext.UserMonthlyBudgets
                .Any(b => b.UserId == u.Id && b.Date == beginningOfMonth))
            .Select(u => u.Id)
            .ToListAsync(ct);

        var errors = new List<string>();
        
        foreach (var userId in usersWithOutBudget)
        {
            var result = await GenerateMonthlyBudgetAsync(userId, ct);

            if (result.IsFailed)
            {
                errors.Add($"Failed to generate monthly budget for {userId}.");
            }
        }
            
        return errors.Count == 0 ? Result.Ok() : Result.Fail(errors);
    }

    public async Task<Result<UserMonthlyBudgetDto>> GetUserCurrentMonthlyBudgetAsync(Guid userId, CancellationToken ct)
    {
        var beginningOfMonth = ToBeginningOfMonth(DateTime.UtcNow);
        
        var userMonthlyBudget = await _dbContext.UserMonthlyBudgets
            .FirstOrDefaultAsync(i => i.UserId == userId
                && i.Date == beginningOfMonth, ct);

        if (userMonthlyBudget == null)
        {
            return Result.Fail("UserMonthlyBudget not found");
        }

        var userMonthlyBudgetDto = new UserMonthlyBudgetDto
        {
            Name = $"Budget for '{userMonthlyBudget.Date.Month}-{userMonthlyBudget.Date.Year}'",
            Id = userMonthlyBudget.Id,
            CreatedAt = userMonthlyBudget.CreatedAt,
            UpdatedAt = userMonthlyBudget.UpdatedAt,
            TotalBudget = userMonthlyBudget.TotalBudget,
            UserId = userMonthlyBudget.UserId,
        };
        
        return Result.Ok(userMonthlyBudgetDto);
    }

    public async Task<Result<PaginatedResponse<UserMonthlyBudgetDto>>> GetUserMonthlyBudgetHistoryAsync(PageQueryFilter filter, Guid userId, CancellationToken ct)
    {
        var beginningOfMonth = ToBeginningOfMonth(DateTime.UtcNow);
        
        var baseQuery = _dbContext.UserMonthlyBudgets
            .Where(i => i.UserId == userId
                && i.Date < beginningOfMonth);
        
        var itemsCount = await baseQuery.CountAsync(ct);
        
        var previousUserMonthlyBudgets = await baseQuery
            .Select(x => new UserMonthlyBudgetDto
            {
                Name = $"Budget for '{x.Date.Month}-{x.Date.Year}'",
                Id = x.Id,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                TotalBudget = x.TotalBudget,
                UserId = x.UserId
            })
            .Paginate(filter.PageNumber, filter.PageSize)
            .ToListAsync(ct);
        
        var result = new PaginatedResponse<UserMonthlyBudgetDto>(previousUserMonthlyBudgets, filter.PageNumber, filter.PageSize, itemsCount);
        
        return Result.Ok(result);
    }

    public async Task<Result> UpdateUserMonthlyBudgetAsync(Guid userId, CancellationToken ct)
    {
        var beginningOfMonth = ToBeginningOfMonth(DateTime.UtcNow);
        
        var userMonthlyBudget = await _dbContext.UserMonthlyBudgets
            .FirstOrDefaultAsync(i => i.UserId == userId
                && i.Date == beginningOfMonth, ct);

        if (userMonthlyBudget == null)
        {
            var result = await GenerateMonthlyBudgetAsync(userId, ct);

            return result.IsFailed ? Result.Fail("UserMonthlyBudget not found, create new one failed") : Result.Ok();
        }
        
        var userMonthlyBudgetAmount = await _dbContext.Incomes
            .Where(i => i.UserId == userId
                && i.IsActiveThisMonth ==  true)
            .SumAsync(i => i.Amount, ct);    

        userMonthlyBudget.TotalBudget = userMonthlyBudgetAmount;
        userMonthlyBudget.UpdatedAt = DateTime.UtcNow;
        
        await _dbContext.SaveChangesAsync(ct);
        
        return Result.Ok();
    }

    private static DateOnly ToBeginningOfMonth(DateTime date)
    {
         return new DateOnly(date.Year, date.Month, 1);
    }
}