using FinanceTracker.API.Extensions;
using FinanceTracker.Domain.Dtos.Account;
using FinanceTracker.Domain.Interfaces;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.API.Endpoints;

public static class AccountsApi
{
    public static IEndpointRouteBuilder MapAccountsApi(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api")
            .WithTags("Accounts")
            .AllowAnonymous();

        group.MapPost("/register", async (IAccountService accountService, CancellationToken ct, [FromBody] RegisterDto dto) =>
        {
            var result = await accountService.RegisterAsync(dto, ct);

            return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Errors);
        })
        .WithRequestValidation<RegisterDto>()
        .Produces(StatusCodes.Status200OK)
        .Produces<IList<IError>>(StatusCodes.Status400BadRequest, "application/json")
        .WithName("RegisterAccount")
        .WithSummary("Registers a new account")
        .WithDescription("Creates a new user account based on the provided registration details.")
        .WithOpenApi();

        group.MapPost("/login", async (IAccountService accountService, CancellationToken ct, [FromBody] LoginDto dto) =>
        {
            var result = await accountService.LoginAsync(dto, ct);

            return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Errors);
        })
        .Produces<string>(StatusCodes.Status200OK, "application/json")
        .Produces<IList<IError>>(StatusCodes.Status400BadRequest, "application/json")
        .WithName("LoginAccount")
        .WithSummary("Logs in a user")
        .WithDescription("Authenticates a user and returns a JWT token if the credentials are valid.")
        .WithOpenApi();

        return app;
    }
}