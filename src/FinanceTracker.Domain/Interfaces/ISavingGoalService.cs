using FinanceTracker.Domain.Common.Pagination;
using FinanceTracker.Domain.Dtos.SavingGoals;
using FinanceTracker.Domain.Dtos.Transactions;
using FluentResults;

namespace FinanceTracker.Domain.Interfaces;

public interface ISavingGoalService
{
    Task<Result<PaginatedResponse<SavingGoalDto>>> GetSavingGoalsAsync(PageQueryFilter filter);
    Task<Result<PaginatedResponse<TransactionDto>>> GetSavingGoalTransactionsAsync(int id, PageQueryFilter filter);

    Task<Result<int>> CreateSavingGoalAsync(CreateSavingGoalDto dto);

    // Task<Result> UpdateSavingGoalAsync(int id);
    Task<Result> DeleteSavingGoalAsync(int id);
}