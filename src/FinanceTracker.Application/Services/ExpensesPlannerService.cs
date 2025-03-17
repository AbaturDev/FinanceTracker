using FinanceTracker.Application.Mappers;
using FinanceTracker.Application.Utils;
using FinanceTracker.Domain.Common.Pagination;
using FinanceTracker.Domain.Dtos.ExpensesPlanners;
using FinanceTracker.Domain.Dtos.Transactions;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Entities.Owned;
using FinanceTracker.Domain.Interfaces;
using FinanceTracker.Infrastructure.Context;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Application.Services;

public class ExpensesPlannerService : IExpensesPlannerService
{
    private readonly FinanceTrackerDbContext _dbContext;
    private readonly IUserContextService _userContext;

    public ExpensesPlannerService(FinanceTrackerDbContext dbContext, IUserContextService userContext)
    {
        _dbContext = dbContext;
        _userContext = userContext;
    }
    
    public async Task<Result<PaginatedResponse<ExpensesPlannerDto>>> GetExpensesAsync(PageQueryFilter filter, CancellationToken ct)
    {
        var userId = _userContext.GetCurrentUserId();
        
        var baseQuery = _dbContext.ExpensesPlanners
            .Where(e => e.UserId == userId);

        var totalItemsCount = await baseQuery.CountAsync(ct);
        
        var expenses = await baseQuery
            .Select(e => new ExpensesPlannerDto
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                Name = e.Name,
                Budget = e.Budget,
                SpentAmount = e.SpentAmount,
                OriginalExchangeRate = ExchangeRateMapper.MapToExchangeRateDto(e.OriginalExchangeRate),
                Category = CategoryMapper.MapToCategoryDto(e.Category),
                ResetInterval = e.ResetInterval,
                UserId = e.UserId,
            })
            .Paginate(filter.PageNumber, filter.PageSize)
            .ToListAsync(ct);

        var result = new PaginatedResponse<ExpensesPlannerDto>(expenses, filter.PageNumber, filter.PageSize, totalItemsCount);
        
        return Result.Ok(result);
    }

    public async Task<Result<PaginatedResponse<TransactionDto>>> GetExpensesPlannerTransactionsAsync(int id, PageQueryFilter filter, CancellationToken ct)
    {
        var userId = _userContext.GetCurrentUserId();
        
        var expensePlanner = await _dbContext.ExpensesPlanners
            .Include(e => e.Transactions)
            .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId, ct);

        if (expensePlanner == null)
        {
            return Result.Fail("ExpensesPlanner not found");
        }

        if (expensePlanner.Transactions == null)
        {
            return Result.Fail("ExpensesPlanner transactions not found");
        }
        
        var totalItemsCount = expensePlanner.Transactions.Count;
        
        var transactions = expensePlanner.Transactions
            .Select(t => new TransactionDto
            {
                Id = t.Id,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt,
                Name = t.Name,
                Description = t.Description,
                Amount = t.Amount,
                ExchangeRate = ExchangeRateMapper.MapToExchangeRateDto(t.ExchangeRate),
                TransactionSource = t.TransactionSource,
                UserId = t.UserId,
            })
            .Paginate(filter.PageNumber, filter.PageSize)
            .ToList();
        
        var result = new PaginatedResponse<TransactionDto>(transactions, filter.PageNumber, filter.PageSize, totalItemsCount);
        
        return Result.Ok(result);
    }

    public async Task<Result<int>> CreateExpensesPlannerAsync(CreateExpensesPlannerDto dto, CancellationToken ct)
    {
        //zmienic i pobrac calego usera, zeby miec tez jego currency code!!!
        var userId = _userContext.GetCurrentUserId();

        if (userId == null)
        {
            return Result.Fail("User does not exist");
        }

        var expensePlanner = new ExpensesPlanner
        {
            Name = dto.Name,
            Budget = dto.Budget,
            SpentAmount = 0,
            ResetInterval = dto.ResetInterval,
            UserId = userId.Value,
        };
        
        
        //mmoze zrobic utilsa bo praktycznie wszedzie bedzie to samo srpawdzaanie + dodac cache
        if (dto.CurrencyCode == "PLN")
        {
            var exchangeRate = new ExchangeRate
            {
                CurrencyCode = dto.CurrencyCode,
                Mid = 1,
                Date = DateOnly.FromDateTime(DateTime.UtcNow),
            };
            
            expensePlanner.OriginalExchangeRate = exchangeRate;
        }

        _dbContext.ExpensesPlanners.Add(expensePlanner);
        await _dbContext.SaveChangesAsync(ct);
        
        return Result.Ok();
    }

    public async Task<Result> DeleteExpensesPlannerAsync(int id, CancellationToken ct)
    {
        var userId = _userContext.GetCurrentUserId();

        var expensePlanner = await _dbContext.ExpensesPlanners
            .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId, ct);

        if (expensePlanner == null)
        {
            return Result.Fail("ExpensesPlanner not found");
        }

        _dbContext.ExpensesPlanners.Remove(expensePlanner);
        await _dbContext.SaveChangesAsync(ct);
        
        return Result.Ok();
    }
}