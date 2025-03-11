using FinanceTracker.Application.Common.Pagination;
using FinanceTracker.Domain.Dtos.ExpensesPlanners;
using FinanceTracker.Domain.Dtos.Transactions;
using FluentResults;

namespace FinanceTracker.Domain.Interfaces;

public interface IExpensesPlannerService
{
    Task<Result<PaginatedResponse<ExpensesPlannerDto>>> GetExpensesAsync(PageQueryFilter filter);
    Task<Result<PaginatedResponse<TransactionDto>>> GetExpensesPlannerTransactionsAsync(int id, PageQueryFilter filter);

    Task<Result<int>> CreateExpensesPlannerAsync(CreateExpensesPlannerDto dto);

    //Task<Result> UpdateExpensesPlannerAsync(int id);
    Task<Result> DeleteExpensesPlannerAsync(int id);
}