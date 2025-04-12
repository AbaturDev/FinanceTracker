using Bogus;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Infrastructure.Context;
using Microsoft.AspNetCore.Identity;

namespace FinanceTracker.Application.Seeders;

public class UsersSeeder
{
    public static void Seed(FinanceTrackerDbContext dbContext, int userCount)
    {
        if (dbContext.Users.Any())
        {
            return;
        }

        var passwordHasher = new PasswordHasher<User>();
        var faker = new Faker<User>("pl")
            .RuleFor(u => u.CurrencyCode, "PLN");
        
        var users = faker.Generate(userCount);
        
        dbContext.Users.AddRange(users);
        dbContext.SaveChanges();
    }
}