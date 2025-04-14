using Bogus;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Infrastructure.Context;

namespace FinanceTracker.Application.Seeders;

public class UserMonthlyBudgetsSeeder
{
    public static void Seed(FinanceTrackerDbContext dbContext, int budgetsCount)
    {
        if (dbContext.UserMonthlyBudgets.Any())
        {
            return;
        }

        var sampleUsers = dbContext.Users.ToList();

        var faker = new Faker<UserMonthlyBudget>()
            .RuleFor(s => s.Date, f => f.Date.SoonDateOnly())
            .RuleFor(s => s.TotalBudget, f => f.Finance.Amount())
            .RuleFor(s => s.TotalExpenses, f => f.Finance.Amount())
            .RuleFor(s => s.CurrencyCode, f => f.PickRandom(sampleUsers).CurrencyCode)
            .RuleFor(s => s.UserId, f => f.PickRandom(sampleUsers).Id)
            .RuleFor(s => s.User, f => f.PickRandom(sampleUsers));

        var userMonthlyBudgets = faker.Generate(budgetsCount);

        dbContext.UserMonthlyBudgets.AddRange(userMonthlyBudgets);
        dbContext.SaveChanges();

    }
}