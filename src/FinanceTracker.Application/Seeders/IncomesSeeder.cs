using Bogus;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Infrastructure.Context;

namespace FinanceTracker.Application.Seeders
{
    public class IncomesSeeder
    {
        public static void Seed(FinanceTrackerDbContext dbContext, int incomesCount)
        {
            if (dbContext.Incomes.Any())
            {
                return;
            }

            var sampleUsers = dbContext.Users.ToList();

            var faker = new Faker<Income>()
                .RuleFor(i => i.Name, f => f.Name.FullName())
                .RuleFor(i => i.Amount, f => f.Finance.Amount())
                .RuleFor(i => i.IsActive, f => f.Random.Bool())
                .RuleFor(i => i.CurrencyCode, f => f.PickRandom(sampleUsers).CurrencyCode)
                .RuleFor(i => i.UserId, f => f.PickRandom(sampleUsers).Id)
                .RuleFor(i => i.User, f => f.PickRandom(sampleUsers));

            var incomes = faker.Generate(incomesCount);

            dbContext.Incomes.AddRange(incomes);
            dbContext.SaveChanges();
        }
    }
}