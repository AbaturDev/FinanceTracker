using FinanceTracker.Domain.Common.Pagination;
using FinanceTracker.Domain.Dtos.SavingGoals;
using FinanceTracker.Domain.Dtos.Transactions;
using FluentResults;

namespace FinanceTracker.Domain.Interfaces;

public interface ISavingGoalService
{
    Task<Result<PaginatedResponse<SavingGoalDto>>> GetSavingGoalsAsync(PageQueryFilter filter, CancellationToken ct);
    Task<Result<PaginatedResponse<TransactionDto>>> GetSavingGoalTransactionsAsync(int id, PageQueryFilter filter, CancellationToken ct);

    Task<Result<int>> CreateSavingGoalAsync(CreateSavingGoalDto dto, CancellationToken ct);

    Task<Result> UpdateSavingGoalAsync(UpdateSavingGoalDto dto, int id, CancellationToken ct);
    Task<Result> DeleteSavingGoalAsync(int id, CancellationToken ct);
}