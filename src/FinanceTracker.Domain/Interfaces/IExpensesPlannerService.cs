using FinanceTracker.Domain.Common.Pagination;
using FinanceTracker.Domain.Dtos.ExpensesPlanners;
using FinanceTracker.Domain.Dtos.Transactions;
using FluentResults;

namespace FinanceTracker.Domain.Interfaces;

public interface IExpensesPlannerService
{
    Task<Result<PaginatedResponse<ExpensesPlannerDto>>> GetExpensesPlannersAsync(PageQueryFilter filter, CancellationToken ct);
    Task<Result<PaginatedResponse<TransactionDto>>> GetExpensesPlannerTransactionsAsync(int id, PageQueryFilter filter, CancellationToken ct);

    Task<Result<int>> CreateExpensesPlannerAsync(CreateExpensesPlannerDto dto, CancellationToken ct);

    //Task<Result> UpdateExpensesPlannerAsync(int id, CancellationToken ct);
    Task<Result> DeleteExpensesPlannerAsync(int id, CancellationToken ct);
}