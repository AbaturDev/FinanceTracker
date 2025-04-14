using Bogus;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Infrastructure.Context;

namespace FinanceTracker.Application.Seeders;

public static class UserMonthlyBudgetsSeeder
{
    public static void Seed(FinanceTrackerDbContext dbContext, int userMonthlyBudgetsCount)
    {
        if (dbContext.UserMonthlyBudgets.Any())
        {
            return;
        }

        var sampleUsers = dbContext.Users.ToList();

        var faker = new Faker<UserMonthlyBudget>()
            .RuleFor(s => s.TotalBudget, f => f.Finance.Amount(5000, 10000))
            .RuleFor(s => s.TotalExpenses, f => f.Finance.Amount(0, 5000));

        foreach (var user in sampleUsers)
        {
            var budgets = faker.Generate(userMonthlyBudgetsCount)
                .Select(x =>
                {
                    x.Date = GenerateRandomDate();
                    x.CurrencyCode = user.CurrencyCode;
                    x.UserId = user.Id;
                    x.User = user;
                    
                    return x;
                }).ToList();
            
            dbContext.UserMonthlyBudgets.AddRange(budgets);
        }
        
        dbContext.SaveChanges();
    }

    private static DateOnly GenerateRandomDate()
    {
        var currentYear = DateTime.UtcNow.Year;
        var currentMonth = DateTime.UtcNow.Month;

        var random = new Random();

        DateOnly randomDate;
        do
        {
            var randomYear = currentYear - random.Next(0, 6);

            var randomMonth = random.Next(1, 13);

            randomDate = new DateOnly(randomYear, randomMonth, 1);
        }
        while (randomDate == new DateOnly(currentYear, currentMonth, 1));

        return randomDate;
    }
}