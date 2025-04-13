using FinanceTracker.Client.Dtos.Common;
using FinanceTracker.Client.Dtos.ExpensesPlanners;
using FinanceTracker.Client.Dtos.Transactions;

namespace FinanceTracker.Client.Interfaces;

public interface IExpensesPlannerService
{
    Task<PaginatedResponse<ExpensesPlannerDto>?> GetExpensesPlannersAsync(int pageSize, int pageNumber, CancellationToken ct);
    Task<PaginatedResponse<TransactionDto>?> GetExpensesPlannerTransactionsAsync(int id, int pageSize, int pageNumber, CancellationToken ct);
    Task<RequestResult> CreateExpensesPlannerAsync(CreateExpensesPlannerDto dto, CancellationToken ct);
    Task<RequestResult> UpdateExpensesPlannerAsync(UpdateExpensesPlannerDto dto, int id, CancellationToken ct);
    Task<RequestResult> DeleteExpensesPlannerAsync(int id, CancellationToken ct);
    Task<RequestResult> AddTransactionAsync(CreateTransactionDto dto, int id, CancellationToken ct);
}