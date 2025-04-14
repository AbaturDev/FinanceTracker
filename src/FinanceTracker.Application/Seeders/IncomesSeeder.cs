using Bogus;
using FinanceTracker.Application.Common;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Infrastructure.Context;

namespace FinanceTracker.Application.Seeders;

public static class IncomesSeeder
{
    public static void Seed(FinanceTrackerDbContext dbContext, int incomesCount)
    {
        if (dbContext.Incomes.Any())
        {
            return;
        }
        
        var sampleUsers = dbContext.Users.ToList();

        var faker = new Faker<Income>()
            .RuleFor(i => i.CurrencyCode, f => f.PickRandom(CurrencyCodeTable.Currencies))
            .RuleFor(i => i.Name, f => f.Lorem.Word())
            .RuleFor(i => i.Amount, f => f.Finance.Amount(100, 10000))
            .RuleFor(i => i.IsActive, f => f.Random.Bool());

        foreach (var user in sampleUsers)
        {
            var incomes = faker.Generate(incomesCount)
                .Select(x =>
                {
                    x.UserId = user.Id;
                    x.User = user;
                    return x;
                }).ToList();
            
            dbContext.Incomes.AddRange(incomes);
        }

        dbContext.SaveChanges();
    }
}