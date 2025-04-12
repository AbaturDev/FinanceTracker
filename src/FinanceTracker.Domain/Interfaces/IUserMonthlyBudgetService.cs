using FinanceTracker.Domain.Common.Pagination;
using FinanceTracker.Domain.Dtos.UserMonthlyBudgets;
using FluentResults;

namespace FinanceTracker.Domain.Interfaces;

public interface IUserMonthlyBudgetService
{
    Task<Result> GenerateMonthlyBudgetAsync(Guid userId, CancellationToken ct);
    Task<Result> EnsureCreatedAsync(CancellationToken ct);
    Task<Result<UserMonthlyBudgetDto>> GetUserCurrentMonthlyBudgetAsync(CancellationToken ct);
    Task<Result<PaginatedResponse<UserMonthlyBudgetDto>>> GetUserMonthlyBudgetHistoryAsync(PageQueryFilter filter, CancellationToken ct);
    Task<Result> UpdateUserMonthlyBudgetAsync(Guid userId, CancellationToken ct);
}