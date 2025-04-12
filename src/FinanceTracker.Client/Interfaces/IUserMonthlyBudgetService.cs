using FinanceTracker.Client.Dtos.Budgets;
using FinanceTracker.Client.Dtos.Common;

namespace FinanceTracker.Client.Interfaces;

public interface IUserMonthlyBudgetService
{
    Task<UserMonthlyBudgetDto?> GetUserCurrentMonthlyBudgetAsync(CancellationToken ct);
    Task<PaginatedResponse<UserMonthlyBudgetDto>?> GetUserMonthlyBudgetsHistoryAsync(int pageSize, int pageNumber, CancellationToken ct);
}