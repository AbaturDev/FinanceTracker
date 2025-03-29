using FinanceTracker.Domain.Common.Pagination;
using FinanceTracker.Domain.Dtos.UserMonthlyBudgets;
using FluentResults;

namespace FinanceTracker.Domain.Interfaces;

public interface IUserMonthlyBudgetService
{
    Task<Result> GenerateMonthlyBudget(Guid userId, CancellationToken ct);
    Task<Result> EnsureCreated(CancellationToken ct);
    Task<Result<UserMonthlyBudgetDto>> GetUserCurrentMonthlyBudget(Guid userId, CancellationToken ct);
    Task<Result<PaginatedResponse<UserMonthlyBudgetDto>>> GetUserMonthlyBudgetHistory(PageQueryFilter filter, Guid userId, CancellationToken ct);
    Task<Result> UpdateUserMonthlyBudget(Guid userId, CancellationToken ct);
}