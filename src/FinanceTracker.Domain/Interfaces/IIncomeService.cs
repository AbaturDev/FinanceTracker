using FinanceTracker.Domain.Common.Pagination;
using FinanceTracker.Domain.Dtos.Incomes;
using FluentResults;

namespace FinanceTracker.Domain.Interfaces;

public interface IIncomeService
{
    Task<Result<PaginatedResponse<IncomeDto>>> GetIncomesAsync(PageQueryFilter filter, CancellationToken ct);

    Task<Result<int>> CreateIncomeAsync(CreateIncomeDto dto, CancellationToken ct);

    // Task<Result> UpdateIncomeAsync(int id, CancellationToken ct);
    Task<Result> DeleteIncomeAsync(int id, CancellationToken ct);
}