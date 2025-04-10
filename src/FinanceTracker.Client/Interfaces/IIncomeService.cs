using FinanceTracker.Client.Dtos.Common;
using FinanceTracker.Client.Dtos.Incomes;

namespace FinanceTracker.Client.Interfaces;

public interface IIncomeService
{
    Task<PaginatedResponse<IncomeDto>?> GetIncomesAsync(int pageNumber, int pageSize, CancellationToken ct);
    Task<RequestResult> CreateIncomeAsync(CreateIncomeDto dto, CancellationToken ct);
    Task<RequestResult> UpdateIncomeAsync(UpdateIncomeDto dto, int id, CancellationToken ct);
    Task<RequestResult> DeleteIncomeAsync(int id, CancellationToken ct);
}