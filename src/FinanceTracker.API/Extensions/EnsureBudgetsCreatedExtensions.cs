using FinanceTracker.Domain.Interfaces;

namespace FinanceTracker.API.Extensions;

public static class EnsureBudgetsCreatedExtensions
{
    public static async Task EnsureMonthlyBudgetsCreatedAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var userMonthlyBudgetService = scope.ServiceProvider.GetRequiredService<IUserMonthlyBudgetService>();

        await userMonthlyBudgetService.EnsureCreatedAsync(CancellationToken.None);
    }
}