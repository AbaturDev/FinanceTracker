using FinanceTracker.Application.Mappers;
using FinanceTracker.Application.Utils;
using FinanceTracker.Domain.Common.Pagination;
using FinanceTracker.Domain.Dtos.ExpensesPlanners;
using FinanceTracker.Domain.Dtos.Transactions;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Entities.Owned;
using FinanceTracker.Domain.Interfaces;
using FinanceTracker.Infrastructure.Context;
using FinanceTracker.NbpRates.Services.Interfaces;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Application.Services;

public class ExpensesPlannerService : IExpensesPlannerService
{
    private readonly FinanceTrackerDbContext _dbContext;
    private readonly IUserContextService _userContext;
    private readonly INbpRateService _nbpRateService;

    public ExpensesPlannerService(FinanceTrackerDbContext dbContext, IUserContextService userContext, INbpRateService nbpRateService)
    {
        _dbContext = dbContext;
        _userContext = userContext;
        _nbpRateService = nbpRateService;
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
                CurrencyCode = e.CurrencyCode,
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
                OriginalAmount = t.OriginalAmount,
                CalculatedAmount = t.CalculatedAmount,
                BudgetExchangeRate = ExchangeRateMapper.MapToExchangeRateDto(t.BudgetExchangeRate),
                TargetExchangeRate = ExchangeRateMapper.MapToExchangeRateDto(t.TargetExchangeRate),
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
        
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId, ct);

        if (user == null)
        {
            throw new ArgumentNullException(nameof(user), "User is null");
        }

        var expensePlanner = new ExpensesPlanner
        {
            Name = dto.Name,
            Budget = dto.Budget,
            SpentAmount = 0,
            ResetInterval = dto.ResetInterval,
            UserId = user.Id,
            CurrencyCode = dto.CurrencyCode ?? user.CurrencyCode,
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

    public async Task<Result<int>> AddTransactionAsync(int id, CreateTransactionDto dto, CancellationToken ct)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync(ct);
        try
        {
            var userId = _userContext.GetCurrentUserId();

            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId, ct);

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User is null");
            }
        
            var expensesPlanner = await _dbContext.ExpensesPlanners
                .Include(e => e.Transactions)
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId, ct);

            if (expensesPlanner == null)
            {
                return Result.Fail("ExpensesPlanner not found");
            }
            
            var firstDayOfMonth = new DateOnly(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
            
            var userMonthlyBudget = await _dbContext.UserMonthlyBudgets
                .FirstOrDefaultAsync(u => u.UserId == userId
                    && u.Date == firstDayOfMonth, ct);

            if (userMonthlyBudget == null)
            {
                return Result.Fail("UserMonthlyBudget not found");
            }

            if ((userMonthlyBudget.TotalBudget - userMonthlyBudget.TotalExpenses) < dto.Amount)
            {
                return Result.Fail("Not enough money for this transaction");
            }

            var budgetNbpRequest = await _nbpRateService.GetExchangeRateAsync(userMonthlyBudget.CurrencyCode);
            var expensesNbpRequest = await _nbpRateService.GetExchangeRateAsync(expensesPlanner.CurrencyCode);
            
            if (budgetNbpRequest == null || expensesNbpRequest == null)
            {
                return Result.Fail("Exchange rate not found");
            }
            
            var budgetExchangeRate = new ExchangeRate
            {
                CurrencyCode = userMonthlyBudget.CurrencyCode,
                Mid = budgetNbpRequest.Mid,
                Date = budgetNbpRequest.Date,
            };
            
            var targetExchangeRate = new ExchangeRate
            {
                CurrencyCode = expensesPlanner.CurrencyCode,
                Mid = expensesNbpRequest.Mid,
                Date = expensesNbpRequest.Date,
            };
            
            var calculatedAmount =  dto.Amount;
            if (expensesPlanner.CurrencyCode != userMonthlyBudget.CurrencyCode)
            {
                var amountInPln = dto.Amount * budgetExchangeRate.Mid;
                calculatedAmount = amountInPln / targetExchangeRate.Mid;
            }
            
            var expensesPlannerTransaction = new Transaction
            {
                Name = dto.Name,
                Description = dto.Description,
                UserId = user.Id,
                UserMonthlyBudgetId = userMonthlyBudget.Id,
                OriginalAmount = dto.Amount,
                CalculatedAmount = calculatedAmount,
                BudgetExchangeRate = budgetExchangeRate,
                TargetExchangeRate = targetExchangeRate,
            };
            
            expensesPlanner.Transactions?.Add(expensesPlannerTransaction);
            expensesPlanner.SpentAmount += calculatedAmount;
            expensesPlanner.UpdatedAt = DateTime.UtcNow;

            userMonthlyBudget.TotalExpenses += dto.Amount;
            userMonthlyBudget.UpdatedAt = DateTime.UtcNow;
            
            await _dbContext.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);

            return Result.Ok(expensesPlannerTransaction.Id);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(ct);
            return Result.Fail(ex.Message);
        }
    }
}