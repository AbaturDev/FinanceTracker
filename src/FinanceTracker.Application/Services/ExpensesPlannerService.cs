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
    
    public async Task<Result<PaginatedResponse<ExpensesPlannerDto>>> GetExpensesPlannersAsync(PageQueryFilter filter, CancellationToken ct)
    {
        var userId = _userContext.GetCurrentUserId();
        
        var baseQuery = _dbContext.ExpensesPlanners
            .AsNoTracking()
            .Where(e => e.UserId == userId);

        var totalItemsCount = await baseQuery.CountAsync(ct);
        
        var expenses = await baseQuery
            .Select(e => new ExpensesPlannerDto
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                UpdatedAt = e.UpdatedAt,
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
        var userId = _userContext.GetCurrentUserId();

        if (userId == null)
        {
            throw new ArgumentNullException(nameof(userId), "User is null");
        }

        var expensePlanner = new ExpensesPlanner
        {
            Name = dto.Name,
            Budget = dto.Budget,
            SpentAmount = 0,
            ResetInterval = dto.ResetInterval,
            UserId = userId.Value,
        };

        if (!string.IsNullOrEmpty(dto.CategoryName))
        {
            expensePlanner.Category = new Category
            {
                Name = dto.CategoryName,
            };    
        }
        
        await _dbContext.ExpensesPlanners.AddAsync(expensePlanner, ct);
        await _dbContext.SaveChangesAsync(ct);
        
        return Result.Ok(expensePlanner.Id);
    }

    public async Task<Result> UpdateExpensesPlannerAsync(UpdateExpensesPlannerDto dto, int id, CancellationToken ct)
    {
        var userId = _userContext.GetCurrentUserId();
        
        var expensesPlanner = await _dbContext.ExpensesPlanners
            .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId, ct);
        
        if (expensesPlanner == null)
        {
            return Result.Fail("Expenses planner not found");
        }
        
        if (!string.IsNullOrEmpty(dto.Name))
        {
            expensesPlanner.Name = dto.Name;
        }

        if (dto.Budget != null)
        {
            expensesPlanner.Budget = dto.Budget.Value;
        }

        if (!string.IsNullOrEmpty(dto.CategoryName) && expensesPlanner.Category != null)
        {
            expensesPlanner.Category.Name = dto.CategoryName;
        }

        if (dto.ResetInterval != null)
        {
            expensesPlanner.ResetInterval = dto.ResetInterval.Value;
        }
        
        expensesPlanner.UpdatedAt = DateTime.UtcNow;
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