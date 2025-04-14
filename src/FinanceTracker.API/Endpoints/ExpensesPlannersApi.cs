using FinanceTracker.API.Extensions;
using FinanceTracker.Domain.Common.Pagination;
using FinanceTracker.Domain.Dtos.ExpensesPlanners;
using FinanceTracker.Domain.Dtos.Transactions;
using FinanceTracker.Domain.Interfaces;
using FluentResults;

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
            
            return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Errors);
        })
        .WithRequestValidation<PageQueryFilter>()
        .Produces<PaginatedResponse<ExpensesPlannerDto>>(StatusCodes.Status200OK, "application/json")
        .Produces<IList<IError>>(StatusCodes.Status400BadRequest, "application/json")
        .WithName("GetExpensesPlanners")
        .WithSummary("Retrieves a paginated list of expenses planners.")
        .WithDescription("Returns a paginated list of all expenses planners that match the given filter.")
        .WithOpenApi();

        group.MapGet("/{id:int}/transactions",
            async (IExpensesPlannerService expensesPlannerService, [AsParameters] PageQueryFilter filter, int id, CancellationToken ct) =>
        {
            var result = await expensesPlannerService.GetExpensesPlannerTransactionsAsync(id, filter, ct);

            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.Errors);
        })
        .WithRequestValidation<PageQueryFilter>()
        .Produces<PaginatedResponse<TransactionDto>>(StatusCodes.Status200OK, "application/json")
        .Produces<IList<IError>>(StatusCodes.Status404NotFound, "application/json")
        .WithName("GetExpensesPlannerTransactions")
        .WithSummary("Retrieves transactions for a specific expenses planner.")
        .WithDescription("Returns a paginated list of transactions associated with the specified expenses planner.")
        .WithOpenApi();

        group.MapPost("", async (IExpensesPlannerService expensesPlannerService, CancellationToken ct, CreateExpensesPlannerDto dto) =>
        {
            var result = await expensesPlannerService.CreateExpensesPlannerAsync(dto, ct);
            
            return result.IsSuccess ? Results.Created($"/api/expenses-planners/{result.Value}", null) : Results.BadRequest(result.Errors);
        })
        .WithRequestValidation<CreateExpensesPlannerDto>()
        .Produces(StatusCodes.Status201Created)
        .Produces<IList<IError>>(StatusCodes.Status400BadRequest, "application/json")
        .WithName("CreateExpensesPlanner")
        .WithSummary("Creates a new expenses planner.")
        .WithDescription("Creates an expenses planner based on the provided details and returns the created resource's location in header.")
        .WithOpenApi();

        group.MapPut("/{id:int}", async (IExpensesPlannerService expensesPlannerService, CancellationToken ct, UpdateExpensesPlannerDto dto, int id) =>
        {
            var result = await expensesPlannerService.UpdateExpensesPlannerAsync(dto, id, ct);
            
            return result.IsSuccess ? Results.Ok() : Results.NotFound(result.Errors);
        })
        .WithRequestValidation<UpdateExpensesPlannerDto>()
        .Produces(StatusCodes.Status200OK)
        .Produces<IList<IError>>(StatusCodes.Status404NotFound, "application/json")
        .WithName("UpdateExpensesPlanner")
        .WithSummary("Updates an existing expenses planner.")
        .WithDescription("Modifies an expenses planner with the specified ID based on the provided update details.")
        .WithOpenApi();
        
        group.MapDelete("/{id:int}", async (IExpensesPlannerService expensesPlannerService, int id, CancellationToken ct) =>
        {
            var result = await expensesPlannerService.DeleteExpensesPlannerAsync(id, ct);
            
            return result.IsSuccess ? Results.NoContent() : Results.NotFound(result.Errors);
        })
        .Produces(StatusCodes.Status204NoContent)
        .Produces<IList<IError>>(StatusCodes.Status404NotFound, "application/json")
        .WithName("DeleteExpensesPlanner")
        .WithSummary("Deletes an expenses planner.")
        .WithDescription("Removes the specified expenses planner from the system.")
        .WithOpenApi();

        group.MapPost("{id:int}/transactions", async (IExpensesPlannerService expensesPlannerService, CreateTransactionDto dto, int id, CancellationToken ct) =>
        {
            var result = await expensesPlannerService.AddTransactionAsync(id, dto, ct);

            if (result.IsFailed)
            {
                var error = result.Errors.First().Message;

                return error.Contains("not found")
                    ? Results.NotFound(result.Errors)
                    : Results.BadRequest(result.Errors);
            }
            
            return Results.Created($"api/expenses-planner/{id}/transactions/{result.Value}", null);
        })
        .WithRequestValidation<CreateTransactionDto>()
        .Produces(StatusCodes.Status201Created)
        .Produces<IList<IError>>(StatusCodes.Status400BadRequest, "application/json")
        .Produces<IList<IError>>(StatusCodes.Status404NotFound, "application/json")
        .WithName("AddTransactionToExpensesPlanner")
        .WithSummary("Adds a transaction to an expenses planner.")
        .WithDescription("Creates a new transaction under the specified expenses planner and returns its location.")
        .WithOpenApi();
        
        return app;
    }
}