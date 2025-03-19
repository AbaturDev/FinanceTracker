using FinanceTracker.Domain.Common.Pagination;
using FinanceTracker.Domain.Interfaces;

namespace FinanceTracker.API.Endpoints;

public static class ExpensesPlannersApi
{
    public static IEndpointRouteBuilder MapExpensesPlannersApi(this IEndpointRouteBuilder app)
    {
        var group =  app.MapGroup("/api/expenses-planners")
            .WithTags("ExpensesPlanners")
            .RequireAuthorization();

        group.MapGet("", async (IExpensesPlannerService expensesPlannerService, [AsParameters] PageQueryFilter filter, CancellationToken ct) =>
        {
            var result = await expensesPlannerService.GetExpensesPlannersAsync(filter, ct);
            
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.Errors);
        });
        
        return app;
    }
}