using FinanceTracker.Domain.Dtos.Account;
using FinanceTracker.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.API.Endpoints;

public static class AccountsApi
{
    public static IEndpointRouteBuilder MapAccountsApi(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api")
            .WithTags("Accounts")
            .AllowAnonymous();

        group.MapPost("/register",
            async (IAccountService accountService, CancellationToken ct, [FromBody] RegisterDto dto) =>
            {
                var result = await accountService.RegisterAsync(dto, ct);

                return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Errors);
            });

        group.MapPost("/login", async (IAccountService accountService, CancellationToken ct, [FromBody] LoginDto dto) =>
        {
            var result = await accountService.LoginAsync(dto, ct);

            return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Errors);
        });

        return app;
    }
}