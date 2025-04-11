using FinanceTracker.Domain.Common.Pagination;
using FinanceTracker.Domain.Dtos.Incomes;
using FinanceTracker.Domain.Interfaces;
using FluentResults;

namespace FinanceTracker.API.Endpoints;

public static class IncomesApi
{
    public static IEndpointRouteBuilder MapIncomesApi(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/incomes")
            .WithTags("Incomes")
            .RequireAuthorization();

        group.MapGet("", async (IIncomeService incomeService, [AsParameters] PageQueryFilter filter, CancellationToken ct) =>
        {
            var result = await incomeService.GetIncomesAsync(filter, ct);
            
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.Errors);
        })
        .Produces<PaginatedResponse<IncomeDto>>(StatusCodes.Status200OK, "application/json")
        .Produces<IList<IError>>(StatusCodes.Status404NotFound, "application/json")
        .WithName("GetIncomes")
        .WithSummary("Retrieves a paginated list of incomes.")
        .WithDescription("Returns a list of incomes based on the provided pagination filter.")
        .WithOpenApi();

        group.MapPost("", async (IIncomeService incomeService, CreateIncomeDto incomeDto, CancellationToken ct) =>
        {
            var result = await incomeService.CreateIncomeAsync(incomeDto, ct);
            
            return result.IsSuccess ? Results.Created($"/api/incomes/{result.Value}", null) : Results.BadRequest(result.Errors);
        })
        .Produces(StatusCodes.Status201Created)
        .Produces<IList<IError>>(StatusCodes.Status400BadRequest, "application/json")
        .WithName("CreateIncome")
        .WithSummary("Creates a new income entry.")
        .WithDescription("Adds a new income record and returns its location.")
        .WithOpenApi();

        group.MapPut("/{id:int}", async (IIncomeService incomeService, UpdateIncomeDto dto, CancellationToken ct, int id) =>
        {
            var result = await incomeService.UpdateIncomeAsync(dto, id, ct);
            
            return result.IsSuccess ? Results.Ok() : Results.NotFound(result.Errors);
        })
        .Produces(StatusCodes.Status200OK)
        .Produces<IList<IError>>(StatusCodes.Status404NotFound, "application/json")
        .WithName("UpdateIncome")
        .WithSummary("Updates an existing income entry.")
        .WithDescription("Modifies an existing income record based on the provided data.")
        .WithOpenApi();

        group.MapDelete("/{id:int}", async (IIncomeService incomeService, int id, CancellationToken ct) =>
        {
            var result = await incomeService.DeleteIncomeAsync(id, ct);

            return result.IsSuccess ? Results.NoContent() : Results.NotFound(result.Errors);
        })
        .Produces(StatusCodes.Status204NoContent)
        .Produces<IList<IError>>(StatusCodes.Status404NotFound, "application/json")
        .WithName("DeleteIncome")
        .WithSummary("Deletes an income entry.")
        .WithDescription("Removes an income record by its ID.")
        .WithOpenApi();
        
        return app;
    }
}