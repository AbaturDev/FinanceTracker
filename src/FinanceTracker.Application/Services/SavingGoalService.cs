using FinanceTracker.Application.Mappers;
using FinanceTracker.Application.Utils;
using FinanceTracker.Domain.Common.Pagination;
using FinanceTracker.Domain.Dtos.SavingGoals;
using FinanceTracker.Domain.Dtos.Transactions;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Entities.Owned;
using FinanceTracker.Domain.Interfaces;
using FinanceTracker.Infrastructure.Context;
using FinanceTracker.NbpRates.Services.Interfaces;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Application.Services;

public class SavingGoalService : ISavingGoalService
{
    private readonly FinanceTrackerDbContext _dbContext;
    private readonly IUserContextService _userContext;
    private readonly INbpRateService _nbpRateService;

    public SavingGoalService(FinanceTrackerDbContext dbContext, IUserContextService userContext, INbpRateService npRateService)
    {
        _dbContext = dbContext;
        _userContext = userContext;
        _nbpRateService = npRateService;
    }

    public async Task<Result<PaginatedResponse<SavingGoalDto>>> GetSavingGoalsAsync(PageQueryFilter filter, CancellationToken ct)
    {
        var userId = _userContext.GetCurrentUserId();

        var baseQuery = _dbContext.SavingGoals
            .AsNoTracking()
            .Where(e => e.UserId == userId);
        
        var totalItemCount = await baseQuery.CountAsync(ct);

        var savingGoals = await baseQuery
            .Select(e => new SavingGoalDto
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                UpdatedAt = e.UpdatedAt,
                Name = e.Name,
                CurrentBalance = e.CurrentBalance,
                AmountOfMoney = e.AmountOfMoney,
                Goal = e.Goal,
                DueDate = e.DueDate,
                CurrencyCode = e.CurrencyCode,
                UserId = e.UserId,
            })
            .Paginate(filter.PageNumber, filter.PageSize)
            .ToListAsync(ct);
        
        var result = new PaginatedResponse<SavingGoalDto>(savingGoals, filter.PageNumber, filter.PageSize, totalItemCount);
        
        return Result.Ok(result);
    }

    public async Task<Result<PaginatedResponse<TransactionDto>>> GetSavingGoalTransactionsAsync(int id, PageQueryFilter filter, CancellationToken ct)
    {
        var userId = _userContext.GetCurrentUserId();
        
        var savingGoal = await _dbContext.SavingGoals
            .Include(e => e.Transactions)
            .FirstOrDefaultAsync(e=> e.Id == id && e.UserId == userId, ct);

        if (savingGoal == null)
        {
            return Result.Fail("SavingGoal not found");
        }

        if (savingGoal.Transactions == null)
        {
            return Result.Fail("SavingGoal transactions not found");
        }
        
        var totalItemCount = savingGoal.Transactions.Count;
        
        var transactions = savingGoal.Transactions
            .Select(t=> new TransactionDto
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
        
        var result = new PaginatedResponse<TransactionDto>(transactions, filter.PageNumber, filter.PageSize, totalItemCount);
        
        return Result.Ok(result);
    }

    public async Task<Result<int>> CreateSavingGoalAsync(CreateSavingGoalDto dto, CancellationToken ct)
    {
        var userId = _userContext.GetCurrentUserId();
        
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId, ct);

        if (user == null)
        {
            throw new ArgumentNullException(nameof(user), "User is null");
        }

        var savingGoal = new SavingGoal
        {
            Name = dto.Name,
            Goal = dto.Goal,
            User = user,
            UserId = user.Id,
            CurrencyCode = dto.CurrencyCode ?? user.CurrencyCode,
            AmountOfMoney = dto.AmountOfMoney,
            DueDate = dto.DueDate,
        };
        
        await _dbContext.SavingGoals.AddAsync(savingGoal, ct);
        await _dbContext.SaveChangesAsync(ct);
        
        return Result.Ok(savingGoal.Id);
    }

    public async Task<Result> UpdateSavingGoalAsync(UpdateSavingGoalDto dto, int id, CancellationToken ct)
    {
        var userId = _userContext.GetCurrentUserId();
        
        var savingGoal = await _dbContext.SavingGoals
            .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId, ct);

        if (savingGoal == null)
        {
            return Result.Fail("SavingGoal not found");
        }

        if (!string.IsNullOrEmpty(dto.Name))
        {
            savingGoal.Name = dto.Name;
        }

        if (!string.IsNullOrEmpty(dto.Goal))
        {
            savingGoal.Goal = dto.Goal;
        }

        if (dto.AmountOfMoney != null)
        {
            savingGoal.AmountOfMoney = dto.AmountOfMoney.Value;
        }

        if (dto.DueDate != null)
        {
            savingGoal.DueDate = dto.DueDate.Value;
        }
        
        savingGoal.UpdatedAt = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync(ct);
        
        return Result.Ok();
    }

    public async Task<Result> DeleteSavingGoalAsync(int id, CancellationToken ct)
    {
        var userId = _userContext.GetCurrentUserId();
        
        var savingGoal = await _dbContext.SavingGoals
            .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId, ct);

        if (savingGoal == null)
        {
            return Result.Fail("SavingGoal not found");
        }

        _dbContext.SavingGoals.Remove(savingGoal);
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
        
            var savingGoal = await _dbContext.SavingGoals
                .Include(e => e.Transactions)
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId, ct);

            if (savingGoal == null)
            {
                return Result.Fail("SavingGoal not found");
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
            var expensesNbpRequest = await _nbpRateService.GetExchangeRateAsync(savingGoal.CurrencyCode);
            
            if (budgetNbpRequest == null || expensesNbpRequest == null)
            {
                return Result.Fail("Exchange rate not found");
            }
            
            var budgetExchangeRate = new ExchangeRate
            {
                CurrencyCode = userMonthlyBudget.CurrencyCode,
                Mid = budgetNbpRequest.Mid,
                Date = budgetNbpRequest.Date
            };

            var targetExchangeRate = new ExchangeRate
            {
                CurrencyCode = savingGoal.CurrencyCode,
                Mid = expensesNbpRequest.Mid,
                Date = expensesNbpRequest.Date
            };

            var calculatedAmount = dto.Amount;
            if (savingGoal.CurrencyCode != userMonthlyBudget.CurrencyCode)
            {
                var amountInPln = dto.Amount * budgetExchangeRate.Mid;
                calculatedAmount = amountInPln / targetExchangeRate.Mid;
            }

            var savingGoalTransaction = new Transaction
            {
                Name = dto.Name,
                Description = dto.Description,
                UserId = user.Id,
                UserMonthlyBudgetId = userMonthlyBudget.Id,
                OriginalAmount = dto.Amount,
                CalculatedAmount = calculatedAmount,
                BudgetExchangeRate = budgetExchangeRate,
                TargetExchangeRate = targetExchangeRate
            };

            savingGoal.Transactions?.Add(savingGoalTransaction);
            savingGoal.CurrentBalance += calculatedAmount;
            savingGoal.UpdatedAt = DateTime.UtcNow;

            userMonthlyBudget.TotalExpenses += dto.Amount;
            userMonthlyBudget.UpdatedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);

            return Result.Ok(savingGoalTransaction.Id);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(ct);
            return Result.Fail(ex.Message);
        }
    }
}