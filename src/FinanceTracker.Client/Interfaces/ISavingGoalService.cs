using FinanceTracker.Client.Dtos.Common;
using FinanceTracker.Client.Dtos.SavingGoals;
using FinanceTracker.Client.Dtos.Transactions;

namespace FinanceTracker.Client.Interfaces;

public interface ISavingGoalService
{
    Task<PaginatedResponse<SavingGoalDto>?> GetSavingGoalsAsync(int pageSize, int pageNumber, CancellationToken ct);
    Task<PaginatedResponse<TransactionDto>?> GetSavingGoalTransactionsAsync(int id, int pageSize, int pageNumber, CancellationToken ct);
    Task<RequestResult> CreateSavingGoalAsync(CreateSavingGoalDto dto, CancellationToken ct);
    Task<RequestResult> UpdateSavingGoalAsync(UpdateSavingGoalDto dto, int id, CancellationToken ct);
    Task<RequestResult> DeleteSavingGoalAsync(int id, CancellationToken ct);
    Task<RequestResult> AddTransactionAsync(CreateTransactionDto dto, int id, CancellationToken ct);
}