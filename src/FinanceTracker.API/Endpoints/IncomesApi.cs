using FinanceTracker.Application.Services;
using FinanceTracker.Domain.Common.Pagination;
using FinanceTracker.Domain.Dtos.Incomes;
using FinanceTracker.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
        });

        group.MapPost("", async (IIncomeService incomeService, CreateIncomeDto incomeDto, CancellationToken ct) =>
        {
            var result = await incomeService.CreateIncomeAsync(incomeDto, ct);
            
            return result.IsSuccess ? Results.Created($"/api/incomes/{result.Value}", null) : Results.BadRequest(result.Errors);
        });

        group.MapPut("/{id:int}", async (IIncomeService incomeService, UpdateIncomeDto dto, CancellationToken ct, int id) =>
        {
            var result = await incomeService.UpdateIncomeAsync(dto, id, ct);
            
            return result.IsSuccess ? Results.Ok() : Results.NotFound(result.Errors);
        });

        group.MapDelete("/{id:int}", async (IIncomeService incomeService, int id, CancellationToken ct) =>
        {
            var result = await incomeService.DeleteIncomeAsync(id, ct);

            return result.IsSuccess ? Results.NoContent() : Results.NotFound(result.Errors);
        });

        group.MapPut("/{id:int}/change-status", async (IIncomeService incomeService, int id,[FromBody] UpdateIncomeActivityStatusDto dto, CancellationToken ct) =>
        {
            var result = await incomeService.UpdateIncomeActiveStatusAsync(id, dto, ct);

            if (result.IsFailed)
            {
                var error = result.Errors.First().Message;

                return error.Contains("not found") ? Results.NotFound(result.Errors) : Results.BadRequest(result.Errors);
            }
            
            return Results.Ok();
        });
        
        return app;
    }
}