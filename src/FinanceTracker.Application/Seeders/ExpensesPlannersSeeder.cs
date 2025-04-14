using Bogus;
using FinanceTracker.Application.Common;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Entities.Owned;
using FinanceTracker.Infrastructure.Context;

namespace FinanceTracker.Application.Seeders;

public static class ExpensesPlannersSeeder
{
    public static void Seed(FinanceTrackerDbContext dbContext, int expensesPlannersCount, int transactionsCount)
    {
        if (dbContext.ExpensesPlanners.Any())
        {
            return;
        }

        var sampleUsers = dbContext.Users.ToList();
        var sampleUserMonthlyBudgets = dbContext.UserMonthlyBudgets.ToList();

        var faker = new Faker<ExpensesPlanner>()
            .RuleFor(u => u.Name, f => f.Lorem.Word())
            .RuleFor(u => u.Budget, f => f.Finance.Amount(5000, 10000))
            .RuleFor(u => u.SpentAmount, f => f.Finance.Amount(0, 5000))
            .RuleFor(u => u.CurrencyCode, f => f.PickRandom(CurrencyCodeTable.Currencies));

        var fakerTransactions = new Faker<Transaction>()
            .RuleFor(t => t.Name, f => f.Lorem.Word())
            .RuleFor(t => t.Description, f => f.Lorem.Paragraphs())
            .RuleFor(t => t.OriginalAmount, f => f.Finance.Amount())
            .RuleFor(t => t.CalculatedAmount, f => f.Finance.Amount())
            .RuleFor(t => t.UserMonthlyBudgetId, f => f.PickRandom(sampleUserMonthlyBudgets).Id);
            
        foreach (var user in sampleUsers)
        {
            var expensesPlanners = faker.Generate(expensesPlannersCount)
                .Select(x =>
                {
                    x.User = user;
                    x.UserId = user.Id;
                    x.Category = new Category()
                    {
                        Name = "Product category"
                    };
                    
                    x.Transactions = fakerTransactions.Generate(transactionsCount)
                        .Select(t =>
                        {
                            t.User = user;
                            t.UserId = user.Id;
                            t.BudgetExchangeRate = new ExchangeRate
                            {
                                CurrencyCode = user.CurrencyCode,
                            };
                            t.TargetExchangeRate = new ExchangeRate
                            {
                                CurrencyCode = x.CurrencyCode,
                            };
                            
                            return t;
                        }).ToList();
                    
                    return x;
                })
                .ToList();
            
            dbContext.AddRange(expensesPlanners);
        }

        dbContext.SaveChanges();

    }
}