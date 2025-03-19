using FinanceTracker.Domain.Common.Pagination;
using FinanceTracker.Domain.Dtos.ExpensesPlanners;
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

        group.MapGet("/{id:int}/transactions",
            async (IExpensesPlannerService expensesPlannerService, [AsParameters] PageQueryFilter filter, int id,
                CancellationToken ct) =>
            {
                var result = await expensesPlannerService.GetExpensesPlannerTransactionsAsync(id, filter, ct);

                return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.Errors);
            });

        group.MapPost("", async (IExpensesPlannerService expensesPlannerService, CancellationToken ct, CreateExpensesPlannerDto dto) =>
        {
            var result = await expensesPlannerService.CreateExpensesPlannerAsync(dto, ct);
            
            return result.IsSuccess ? Results.Created($"/api/expenses-planners/{result.Value}", null) : Results.BadRequest(result.Errors);
        });

        group.MapPut("/{id:int}", async (IExpensesPlannerService expensesPlannerService, CancellationToken ct, UpdateExpensesPlannerDto dto, int id) =>
        {
            var result = await expensesPlannerService.UpdateExpensesPlannerAsync(dto, id, ct);
            
            return result.IsSuccess ? Results.Ok() : Results.NotFound(result.Errors);
        });
        
        group.MapDelete("/{id:int}",
            async (IExpensesPlannerService expensesPlannerService, int id, CancellationToken ct) =>
            {
                var result = await expensesPlannerService.DeleteExpensesPlannerAsync(id, ct);
                
                return result.IsSuccess ? Results.NoContent() : Results.NotFound(result.Errors);
            });
        
        return app;
    }
}