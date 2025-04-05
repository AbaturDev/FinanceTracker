using Azure;
using FinanceTracker.Application.Services;
using FinanceTracker.Domain.Common.Pagination;
using FinanceTracker.Domain.Dtos.SavingGoals;
using FinanceTracker.Domain.Interfaces;

namespace FinanceTracker.API.Endpoints;

public static class SavingGoalApi
{
    public static IEndpointRouteBuilder MapExpensesPlannerApi(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/saving-goals")
            .WithTags("SavingGoals")
            .RequireAuthorization();

        group.MapGet("", 
            async (ISavingGoalService savingGoalService, [AsParameters] PageQueryFilter filter, CancellationToken ct) =>
        {
            var result = await savingGoalService.GetSavingGoalsAsync(filter, ct);

            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.Errors);
        });

        group.MapGet("/{id:int}/transactions",
            async (ISavingGoalService savingGoalService, [AsParameters] PageQueryFilter filter,
                int id, CancellationToken ct) =>
            {
                var result = await savingGoalService.GetSavingGoalTransactionsAsync(id, filter, ct);

                return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.Errors);
            });

        group.MapPost("", async (ISavingGoalService savingGoalService, CancellationToken ct, CreateSavingGoalDto dto) =>
        {
            var result = await savingGoalService.CreateSavingGoalAsync(dto, ct);

            return result.IsSuccess
                ? Results.Created($"/api/saving-goals/{result.Value}", null)
                : Results.BadRequest(result.Errors);
        });

        group.MapPut("/{id:int}",
            async (ISavingGoalService savingGoalService, CancellationToken ct, UpdateSavingGoalDto dto, int id) =>
            {
                var result = await savingGoalService.UpdateSavingGoalAsync(dto, id, ct);

                return result.IsSuccess ? Results.Ok() : Results.NotFound(result.Errors);
            });

        group.MapDelete("/{id:int}",
            async (ISavingGoalService savingGoalService, int id, CancellationToken ct) =>
            {
                var result = await savingGoalService.DeleteSavingGoalAsync(id, ct);

                return result.IsSuccess ? Results.NoContent() : Results.NotFound(result.Errors);
            });
        
        return app;
    }
}