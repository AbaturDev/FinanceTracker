using FinanceTracker.Application.Seeders;
using FinanceTracker.Infrastructure.Context;

namespace FinanceTracker.API.Extensions;

public static class SeedingExtensions
{
    private const int PlannersCount = 100;
    private const int IncomesCount = 100;
    private const int GoalsCount = 100;
    private const int TransactionsCount = 100;
    private const int BudgetsCount = 100;
    private const int UsersCount = 100;
    
    public static WebApplication Seed(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            using var context = scope.ServiceProvider.GetRequiredService<FinanceTrackerDbContext>();

            context.Database.EnsureCreated();

            UsersSeeder.Seed(context, UsersCount);
            ExpensesPlannersSeeder.Seed(context, PlannersCount);
            IncomesSeeder.Seed(context, IncomesCount);
            SavingGoalsSeeder.Seed(context, GoalsCount);
            UserMonthlyBudgetsSeeder.Seed(context, BudgetsCount);
            TransactionsSeeder.Seed(context, TransactionsCount);
        }
        
        return app;
    }
}