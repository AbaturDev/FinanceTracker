using FinanceTracker.Application.Utils;
using FinanceTracker.Domain.Common.Pagination;
using FinanceTracker.Domain.Dtos.UserMonthlyBudgets;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Interfaces;
using FinanceTracker.Infrastructure.Context;
using FinanceTracker.NbpRates.Services.Interfaces;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Application.Services;

public class UserMonthlyBudgetService : IUserMonthlyBudgetService
{
    private readonly FinanceTrackerDbContext _dbContext;
    private readonly INbpRateService _nbpRateService;

    public UserMonthlyBudgetService(FinanceTrackerDbContext dbContext, INbpRateService nbpRateService)
    {
        _dbContext = dbContext;
        _nbpRateService = nbpRateService;
    }

    public async Task<Result> GenerateMonthlyBudgetAsync(Guid userId, CancellationToken ct)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync(ct);
        try
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId, ct);
            if (user == null)
            {
                return Result.Fail("User not found");
            }
            
            var beginningOfMonth = ToBeginningOfMonth(DateTime.UtcNow);

            var budgetAlreadyExist = await _dbContext.UserMonthlyBudgets
                .AnyAsync(b => b.UserId == userId
                               && b.Date == beginningOfMonth, ct);

            if (budgetAlreadyExist)
            {
                return Result.Fail("UserMonthlyBudget already exists");
            }

            var userIncomes = await _dbContext.Incomes
                .Where(i => i.UserId == userId
                            && i.IsActive == true)
                .ToListAsync(ct);

            decimal sum = 0;

            foreach (var income in userIncomes)
            {
                sum += await ConvertToBudgetCurrency(income, user.CurrencyCode);
            }
            
            var budget = new UserMonthlyBudget
            {
                UserId = userId,
                Date = beginningOfMonth,
                TotalBudget = sum,
                CurrencyCode = user.CurrencyCode,
                Incomes = userIncomes
            };
            await _dbContext.UserMonthlyBudgets.AddAsync(budget, ct);
            await _dbContext.SaveChangesAsync(ct);

            foreach (var income in userIncomes)
            {
                income.UserMonthlyBudgets?.Add(budget);
            }
            
            await _dbContext.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);

            return Result.Ok();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(ct);
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
            TotalExpenses = userMonthlyBudget.TotalExpenses,
            CurrencyCode = userMonthlyBudget.CurrencyCode,
            UserId = userMonthlyBudget.UserId
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
                TotalExpenses = x.TotalExpenses,
                CurrencyCode = x.CurrencyCode,
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
            .Include(x => x.Incomes)
            .FirstOrDefaultAsync(x => x.UserId == userId
                                      && x.Date == beginningOfMonth, ct);

        if (userMonthlyBudget == null)
        {
            var result = await GenerateMonthlyBudgetAsync(userId, ct);

            return result.IsFailed ? Result.Fail("UserMonthlyBudget not found, create new one failed") : Result.Ok();
        }

        var userIncomes = await _dbContext.Incomes
            .Where(i => i.UserId == userId
                        && i.IsActive == true)
            .ToListAsync(ct);

        decimal updatedBudget = 0;
        foreach (var income in userIncomes)
        {
            updatedBudget += await ConvertToBudgetCurrency(income, userMonthlyBudget.CurrencyCode); 
        }

        userMonthlyBudget.TotalBudget = updatedBudget;
        userMonthlyBudget.Incomes?.Clear();
        userMonthlyBudget.Incomes = userIncomes;
        userMonthlyBudget.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(ct);

        return Result.Ok();
    }

    private static DateOnly ToBeginningOfMonth(DateTime date)
    {
        return new DateOnly(date.Year, date.Month, 1);
    }

    private async Task<decimal> ConvertToBudgetCurrency(Income income, string currencyCode)
    {
        if (income.CurrencyCode == currencyCode)
        {
            return income.Amount;
        }
                
        var incomeNbpRate = await _nbpRateService.GetExchangeRateAsync(income.CurrencyCode);
        var userCurrencyNbpRate = await _nbpRateService.GetExchangeRateAsync(currencyCode);
                
        if (incomeNbpRate == null || userCurrencyNbpRate == null)
        {
            return 0;
        }

        var incomeInPln = income.Amount * incomeNbpRate.Mid;
        return incomeInPln / userCurrencyNbpRate.Mid;
    }
}