using Bogus;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Infrastructure.Context;
using Microsoft.AspNetCore.Identity;

namespace FinanceTracker.Application.Seeders
{
    public class UsersSeeder
    {
        public static void Seed(FinanceTrackerDbContext dbContext, int userCount)
        {
            if (dbContext.Users.Any())
            {
                return;
            }

            var passwordHasher = new PasswordHasher<User>();
            var faker = new Faker<User>("pl");

            var users = new List<User>();
            users.AddRange(faker.Generate(userCount)
                .Select(u =>
                {
                    u.PasswordHash = passwordHasher.HashPassword(u, "haslohaslo");
                    return u;
                }));

            dbContext.Users.AddRange(users);
            dbContext.SaveChanges();
        }
    }
}