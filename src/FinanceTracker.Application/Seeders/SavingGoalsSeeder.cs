using Bogus;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Infrastructure.Context;

namespace FinanceTracker.Application.Seeders
{
    public class SavingGoalsSeeder
    {
        public static void Seed(FinanceTrackerDbContext dbContext, int goalsCount)
        {
            if (dbContext.SavingGoals.Any())
            {
                return;
            }

            var sampleUsers = dbContext.Users.ToList();

            var faker = new Faker<SavingGoal>()
                .RuleFor(s => s.Name, f => f.Name.FirstName())
                .RuleFor(s => s.CurrentBalance, f => f.Finance.Amount())
                .RuleFor(s => s.AmountOfMoney, f => f.Finance.Amount())
                .RuleFor(s => s.Goal, f => f.Random.Word())
                .RuleFor(s => s.DueDate, f => f.Date.FutureDateOnly())
                .RuleFor(s => s.CurrencyCode, f => f.PickRandom(sampleUsers).CurrencyCode)
                .RuleFor(s => s.UserId, f => f.PickRandom(sampleUsers).Id)
                .RuleFor(s => s.User, f => f.PickRandom(sampleUsers));

            var savingGoals = faker.Generate(goalsCount);

            dbContext.SavingGoals.AddRange(savingGoals);
            dbContext.SaveChanges();
        }
    }
}