using Bogus;
using FinanceTracker.Application.Common;
using FinanceTracker.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace FinanceTracker.Application.Seeders;

public static class UsersSeeder
{
    private const string UserPassword = "Haslo123!";
    public static void Seed(UserManager<User> userManager, int userCount)
    {
        if (userManager.Users.Any())
        {
            return;
        }

        var faker = new Faker<User>("pl")
            .RuleFor(u => u.CurrencyCode, f => f.PickRandom(CurrencyCodeTable.Currencies))
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.UserName, f => f.Person.UserName);
        
        var users = faker.Generate(userCount);

        foreach (var user in users)
        {
            userManager.CreateAsync(user, UserPassword);
        }
    }
}