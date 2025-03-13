using FinanceTracker.Domain.Common.Pagination;
using FinanceTracker.Domain.Dtos.Incomes;
using FluentResults;

namespace FinanceTracker.Domain.Interfaces;

public interface IIncomeService
{
    Task<Result<PaginatedResponse<IncomeDto>>> GetIncomesAsync(PageQueryFilter filter);

    Task<Result<int>> CreateIncomeAsync(CreateIncomeDto dto);

    // Task<Result> UpdateIncomeAsync(int id);
    Task<Result> DeleteIncomeAsync(int id);
}