using FinanceTracker.Application.Seeders;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Infrastructure.Context;
using Microsoft.AspNetCore.Identity;

namespace FinanceTracker.API.Extensions;

public static class SeedingExtensions
{
    private const int UsersCount = 10;
    private const int IncomesCount = 50;
    private const int UserMonthlyBudgetsCount = 30;
    private const int SavingGoalsCount = 100;
    private const int ExpensesPlannerCount = 100;
    private const int TransactionsCount = 30;
    
    public static WebApplication Seed(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<FinanceTrackerDbContext>();
        using var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

        context.Database.EnsureCreated();

        UsersSeeder.Seed(userManager, UsersCount);
        IncomesSeeder.Seed(context, IncomesCount);
        UserMonthlyBudgetsSeeder.Seed(context, UserMonthlyBudgetsCount);
        ExpensesPlannersSeeder.Seed(context, ExpensesPlannerCount, TransactionsCount);
        SavingGoalsSeeder.Seed(context, SavingGoalsCount, TransactionsCount);

        return app;
    }
}