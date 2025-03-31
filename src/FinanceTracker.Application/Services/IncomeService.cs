using FinanceTracker.Application.Mappers;
using FinanceTracker.Application.Utils;
using FinanceTracker.Domain.Common.Pagination;
using FinanceTracker.Domain.Dtos.Incomes;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Interfaces;
using FinanceTracker.Infrastructure.Context;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Application.Services;

public class IncomeService : IIncomeService
{
    private readonly FinanceTrackerDbContext _dbContext;
    private readonly IUserContextService _userContext;
    private readonly IUserMonthlyBudgetService _userMonthlyBudgetService;

    public IncomeService(FinanceTrackerDbContext dbContext, IUserContextService userContext, IUserMonthlyBudgetService userMonthlyBudgetService)
    {
        _dbContext = dbContext;
        _userContext = userContext;
        _userMonthlyBudgetService = userMonthlyBudgetService;
    }
    
    public async Task<Result<PaginatedResponse<IncomeDto>>> GetIncomesAsync(PageQueryFilter filter, CancellationToken ct)
    {
        var userId = _userContext.GetCurrentUserId();
        
        var baseQuery = _dbContext.Incomes
            .Where(i => i.UserId == userId);

        var itemsCount = await baseQuery.CountAsync(ct);

        var incomes = await baseQuery
            .Select(x => new IncomeDto
            {
                Id = x.Id,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                Name = x.Name,
                Amount = x.Amount,
                RegularIncome = x.RegularIncome,
                IsActiveThisMonth = x.IsActiveThisMonth,
                CurrencyCode = x.CurrencyCode,
                UserId = x.UserId,
            })
            .Paginate(filter.PageNumber, filter.PageSize)
            .AsNoTracking()
            .ToListAsync(ct);
            
        var result = new PaginatedResponse<IncomeDto>(incomes, filter.PageNumber, filter.PageSize, itemsCount);
        
        return Result.Ok(result);
    }

    public async Task<Result<int>> CreateIncomeAsync(CreateIncomeDto dto, CancellationToken ct)
    {
        var userId = _userContext.GetCurrentUserId();

        if (userId == null)
        {
            throw new ArgumentNullException(nameof(userId), "User is null");
        }

        var income = new Income
        {
            Name = dto.Name,
            Amount = dto.Amount,
            RegularIncome = dto.RegularIncome,
            IsActiveThisMonth = dto.IsActiveThisMonth,
            UserId = userId.Value,
            CurrencyCode = dto.CurrencyCode,
        };
        
        await _dbContext.Incomes.AddAsync(income, ct);
        await _dbContext.SaveChangesAsync(ct);

        if (income.IsActiveThisMonth)
        {
            await _userMonthlyBudgetService.UpdateUserMonthlyBudgetAsync(userId.Value, ct);
        }
        
        return Result.Ok(income.Id);
    }

    public async Task<Result> UpdateIncomeAsync(UpdateIncomeDto dto, int id, CancellationToken ct)
    {
        var userId = _userContext.GetCurrentUserId();

        if (userId == null)
        {
            throw new ArgumentNullException(nameof(userId), "User is null");
        }
        
        var income = await _dbContext.Incomes
            .FirstOrDefaultAsync(i => i.Id == id && i.UserId == userId, ct);

        if (income == null)
        {
            return Result.Fail("Income not found");
        }

        if (!string.IsNullOrEmpty(dto.Name))
        {
            income.Name = dto.Name;
        }

        if (dto.Amount != null)
        {
            income.Amount = dto.Amount.Value;
        }

        if (dto.RegularIncome != null)
        {
            income.RegularIncome = dto.RegularIncome.Value;
        }

        income.UpdatedAt = DateTime.UtcNow;
        
        await _dbContext.SaveChangesAsync(ct);

        if (income.IsActiveThisMonth)
        {
            await _userMonthlyBudgetService.UpdateUserMonthlyBudgetAsync(userId.Value, ct);
        }
        
        return Result.Ok();
    }

    public async Task<Result> DeleteIncomeAsync(int id, CancellationToken ct)
    {
        var userId = _userContext.GetCurrentUserId();
        
        if (userId == null)
        {
            throw new ArgumentNullException(nameof(userId), "User is null");
        }
        
        var income = await _dbContext.Incomes
            .FirstOrDefaultAsync(i => i.Id == id && i.UserId == userId, ct);

        if (income == null)
        {
            return Result.Fail("Income not found");
        }
        
        _dbContext.Incomes.Remove(income);
        await _dbContext.SaveChangesAsync(ct);
        
        if (income.IsActiveThisMonth)
        {
            await _userMonthlyBudgetService.UpdateUserMonthlyBudgetAsync(userId.Value, ct);
        }
        
        return Result.Ok();
    }

    public async Task<Result> UpdateIncomeActiveStatusAsync(int id, UpdateIncomeActivityStatusDto dto, CancellationToken ct)
    {
        var userId = _userContext.GetCurrentUserId();
        
        if (userId == null)
        {
            throw new ArgumentNullException(nameof(userId), "User is null");
        }
        
        var income = await _dbContext.Incomes
            .FirstOrDefaultAsync(i => i.Id == id && i.UserId == userId, ct);

        if (income == null)
        {
            return Result.Fail("Income not found");
        }

        if (income.RegularIncome)
        {
            return Result.Fail("Income is set as regular income, changing it status is forbidden");
        }
        
        income.IsActiveThisMonth = dto.IsActiveThisMonth;
        income.UpdatedAt = DateTime.UtcNow;
        
        await _dbContext.SaveChangesAsync(ct);
        
        await _userMonthlyBudgetService.UpdateUserMonthlyBudgetAsync(userId.Value, ct);
        
        return Result.Ok();
    }
}