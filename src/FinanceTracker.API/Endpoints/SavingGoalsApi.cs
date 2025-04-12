using FinanceTracker.Domain.Common.Pagination;
using FinanceTracker.Domain.Dtos.SavingGoals;
using FinanceTracker.Domain.Dtos.Transactions;
using FinanceTracker.Domain.Interfaces;
using FluentResults;

namespace FinanceTracker.API.Endpoints;

public static class SavingGoalsApi
{
    public static IEndpointRouteBuilder MapSavingGoalsApi(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/saving-goals")
            .WithTags("SavingGoals")
            .RequireAuthorization();

        group.MapGet("", 
                async (ISavingGoalService savingGoalService, [AsParameters] PageQueryFilter filter, CancellationToken ct) =>
        {
            var result = await savingGoalService.GetSavingGoalsAsync(filter, ct);

            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.Errors);
        })
        .Produces<PaginatedResponse<SavingGoalDto>>(StatusCodes.Status200OK, "application/json")
        .Produces<IList<IError>>(StatusCodes.Status404NotFound, "application/json")
        .WithName("GetSavingGoals")
        .WithSummary("Retrieves a paginated list of saving goals.")
        .WithDescription("Returns a paginated list of all saving goals available for the current user. Supports filtering and pagination.")
        .WithOpenApi();
        

        group.MapGet("/{id:int}/transactions",
            async (ISavingGoalService savingGoalService, [AsParameters] PageQueryFilter filter, int id, CancellationToken ct) =>
        {
            var result = await savingGoalService.GetSavingGoalTransactionsAsync(id, filter, ct);

            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.Errors);
        })
        .Produces<PaginatedResponse<TransactionDto>>(StatusCodes.Status200OK, "application/json")
        .Produces<IList<IError>>(StatusCodes.Status404NotFound, "application/json")
        .WithName("GetSavingGoalTransactions")
        .WithSummary("Gets transactions for a specific saving goal.")
        .WithDescription("Retrieves a paginated list of transactions associated with a specific saving goal.")
        .WithOpenApi();

        group.MapPost("", async (ISavingGoalService savingGoalService, CancellationToken ct, CreateSavingGoalDto dto) =>
        {
            var result = await savingGoalService.CreateSavingGoalAsync(dto, ct);

            return result.IsSuccess
                ? Results.Created($"/api/saving-goals/{result.Value}", null)
                : Results.BadRequest(result.Errors);
        })
        .Produces(StatusCodes.Status201Created)
        .Produces<IList<IError>>(StatusCodes.Status400BadRequest, "application/json")
        .WithName("CreateSavingGoal")
        .WithSummary("Creates a new saving goal.")
        .WithDescription("Creates a new saving goal and returns the location of the newly created resource.")
        .WithOpenApi();

        group.MapPut("/{id:int}",
        async (ISavingGoalService savingGoalService, CancellationToken ct, UpdateSavingGoalDto dto, int id) =>
        {
            var result = await savingGoalService.UpdateSavingGoalAsync(dto, id, ct);

            return result.IsSuccess ? Results.Ok() : Results.NotFound(result.Errors);
        })
        .Produces(StatusCodes.Status200OK)
        .Produces<IList<IError>>(StatusCodes.Status404NotFound, "application/json")
        .WithName("UpdateSavingGoal")
        .WithSummary("Updates a saving goal.")
        .WithDescription("Updates the details of a specific saving goal identified by its ID.")
        .WithOpenApi();

        group.MapDelete("/{id:int}", async (ISavingGoalService savingGoalService, int id, CancellationToken ct) =>
        {
            var result = await savingGoalService.DeleteSavingGoalAsync(id, ct);

            return result.IsSuccess ? Results.NoContent() : Results.NotFound(result.Errors);
        })
        .Produces(StatusCodes.Status204NoContent)
        .Produces<IList<IError>>(StatusCodes.Status404NotFound, "application/json")
        .WithName("DeleteSavingGoal")
        .WithSummary("Deletes a saving goal.")
        .WithDescription("Deletes a specific saving goal by its ID if it exists.")
        .WithOpenApi();
        
        group.MapPost("{id:int}/transactions", async (ISavingGoalService savingGoalService, CreateTransactionDto dto, int id, CancellationToken ct) =>
        {
            var result = await savingGoalService.AddTransactionAsync(id, dto, ct);

            if (result.IsFailed)
            {
                var error = result.Errors.First().Message;

                return error.Contains("not found")
                    ? Results.NotFound(result.Errors)
                    : Results.BadRequest(result.Errors);
            }
            
            return Results.Created($"api/saving-goal/{id}/transactions/{result.Value}", null);
        })
        .Produces(StatusCodes.Status201Created)
        .Produces<IList<IError>>(StatusCodes.Status404NotFound, "application/json")
        .Produces<IList<IError>>(StatusCodes.Status400BadRequest, "application/json")
        .WithName("AddTransactionToSavingGoal")
        .WithSummary("Adds a transaction to a saving goal.")
        .WithDescription("Adds a financial transaction to the specified saving goal by ID. Returns the location of the created transaction.")
        .WithOpenApi();
        
        return app;
    }
}