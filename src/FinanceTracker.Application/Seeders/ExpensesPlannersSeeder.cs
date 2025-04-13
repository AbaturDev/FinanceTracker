using Bogus;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Infrastructure.Context;

namespace FinanceTracker.Application.Seeders;

public class ExpensesPlannersSeeder
{
    public static void Seed(FinanceTrackerDbContext dbContext, int plannersCount)
    {
        if (dbContext.ExpensesPlanners.Any())
        {
            return;
        }

        var sampleUsers = dbContext.Users.ToList();

        var faker = new Faker<ExpensesPlanner>()
            .RuleFor(u => u.Name, f => f.Name.FirstName())
            .RuleFor(u => u.Budget, f => f.Finance.Amount())
            .RuleFor(u => u.SpentAmount, f => f.Finance.Amount())
            .RuleFor(u => u.CurrencyCode, f => f.PickRandom(sampleUsers).CurrencyCode)
            .RuleFor(u => u.UserId, f => f.PickRandom(sampleUsers).Id)
            .RuleFor(u => u.User, f => f.PickRandom(sampleUsers));
        
        var expensesPlanners = faker.Generate(plannersCount);

        dbContext.ExpensesPlanners.AddRange(expensesPlanners);
        dbContext.SaveChanges();

    }
}