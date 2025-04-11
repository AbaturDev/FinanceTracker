using FinanceTracker.Application.Seeders;
using FinanceTracker.Infrastructure.Context;

namespace FinanceTracker.API.Extensions;

public static class SeedingExtensions
{
    private const int PlannersCount = 3;
    private const int IncomesCount = 3;
    private const int GoalsCount = 3;
    private const int TransactionsCount = 3;
    private const int BudgetsCount = 3;
    private const int UsersCount = 3;
    
    public static WebApplication Seed(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            using var context = scope.ServiceProvider.GetRequiredService<FinanceTrackerDbContext>();

            context.Database.EnsureCreated();

            ExpensesPlannersSeeder.Seed(context, PlannersCount);
            IncomesSeeder.Seed(context, IncomesCount);
            SavingGoalsSeeder.Seed(context, GoalsCount);
            TransactionsSeeder.Seed(context, TransactionsCount);
            UserMonthlyBudgetsSeeder.Seed(context, BudgetsCount);
            UsersSeeder.Seed(context, UsersCount);
        }
        
        return app;
    }
}