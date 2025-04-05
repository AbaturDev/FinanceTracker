using FinanceTracker.Domain.Common.Pagination;
using FinanceTracker.Domain.Interfaces;

namespace FinanceTracker.API.Endpoints;

public static class UserMonthlyBudgetsApi
{
    public static IEndpointRouteBuilder MapUserMonthlyBudgetsApi(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/user/{userId:guid}/user-monthly-budgets")
            .WithTags("UserMonthlyBudgets")
            .RequireAuthorization();

        group.MapGet("/current-budget", async (IUserMonthlyBudgetService userMonthlyBudgetService, Guid userId, CancellationToken ct) =>
        {
            var result = await userMonthlyBudgetService.GetUserCurrentMonthlyBudgetAsync(userId, ct);
            
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.Errors);
        });
        
        group.MapGet("/budget-history", async (IUserMonthlyBudgetService userMonthlyBudgetService,[AsParameters] PageQueryFilter filter, Guid userId, CancellationToken ct) =>
        {
            var result = await userMonthlyBudgetService.GetUserMonthlyBudgetHistoryAsync(filter, userId, ct);

            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.Errors);
        });

        return app;
    }
}