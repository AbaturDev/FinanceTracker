using Bogus;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Infrastructure.Context;

namespace FinanceTracker.Application.Seeders;

public class UsersSeeder
{
    public static void Seed(FinanceTrackerDbContext dbContext, int userCount)
    {
        if (dbContext.Users.Any())
        {
            return;
        }

        var faker = new Faker<User>("pl")
            .RuleFor(u => u.CurrencyCode, "PLN");
        
        var users = faker.Generate(userCount);
        
        dbContext.Users.AddRange(users);
        dbContext.SaveChanges();
    }
}