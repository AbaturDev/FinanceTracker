using FinanceTracker.API.Endpoints;

namespace FinanceTracker.API.Extensions;

public static class RegisterEndpointsExtensions
{
    public static void RegisterEndpoints(this IEndpointRouteBuilder app)
    {
        app
            .MapAccountsApi()
            .MapExpensesPlannersApi();
    }
}