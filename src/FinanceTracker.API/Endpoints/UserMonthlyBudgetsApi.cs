using FinanceTracker.Domain.Common.Pagination;
using FinanceTracker.Domain.Dtos.UserMonthlyBudgets;
using FinanceTracker.Domain.Interfaces;
using FluentResults;

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
        })
        .Produces<UserMonthlyBudgetDto>(StatusCodes.Status200OK, "application/json")
        .Produces<IList<IError>>(StatusCodes.Status404NotFound, "application/json")
        .WithName("GetCurrentUserBudget")
        .WithSummary("Retrieves the current monthly budget for a user.")
        .WithDescription("Returns the user's budget for the current month.")
        .WithOpenApi();
        
        group.MapGet("/budget-history", async (IUserMonthlyBudgetService userMonthlyBudgetService,[AsParameters] PageQueryFilter filter, Guid userId, CancellationToken ct) =>
        {
            var result = await userMonthlyBudgetService.GetUserMonthlyBudgetHistoryAsync(filter, userId, ct);

            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.Errors);
        })
        .Produces<PaginatedResponse<UserMonthlyBudgetDto>>(StatusCodes.Status200OK, "application/json")
        .Produces<IList<IError>>(StatusCodes.Status404NotFound, "application/json")
        .WithName("GetUserBudgetHistory")
        .WithSummary("Retrieves the monthly budget history for a user.")
        .WithDescription("Returns a paginated history of the user's budgets from previous months without budget fromm current month ")
        .WithOpenApi();

        return app;
    }
}