using Bogus;
using FinanceTracker.Application.Common;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Entities.Owned;
using FinanceTracker.Infrastructure.Context;

namespace FinanceTracker.Application.Seeders;

public static class SavingGoalsSeeder
{
    public static void Seed(FinanceTrackerDbContext dbContext, int savingGoalsCount, int transactionsCount)
    {
        if (dbContext.ExpensesPlanners.Any())
        {
            return;
        }

        var sampleUsers = dbContext.Users.ToList();
        var sampleUserMonthlyBudgets = dbContext.UserMonthlyBudgets.ToList();

        var faker = new Faker<SavingGoal>()
            .RuleFor(u => u.Name, f => f.Lorem.Word())
            .RuleFor(u => u.AmountOfMoney, f => f.Finance.Amount(5000, 10000))
            .RuleFor(u => u.CurrentBalance, f => f.Finance.Amount(0, 5000))
            .RuleFor(u => u.Goal, f => f.Lorem.Sentence())
            .RuleFor(u => u.DueDate, f => f.Date.FutureDateOnly())
            .RuleFor(u => u.CurrencyCode, f => f.PickRandom(CurrencyCodeTable.Currencies));

        var fakerTransactions = new Faker<Transaction>()
            .RuleFor(t => t.Name, f => f.Lorem.Word())
            .RuleFor(t => t.Description, f => f.Lorem.Paragraphs())
            .RuleFor(t => t.OriginalAmount, f => f.Finance.Amount())
            .RuleFor(t => t.CalculatedAmount, f => f.Finance.Amount())
            .RuleFor(t => t.UserMonthlyBudgetId, f => f.PickRandom(sampleUserMonthlyBudgets).Id);
            
        foreach (var user in sampleUsers)
        {
            var expensesPlanners = faker.Generate(savingGoalsCount)
                .Select(x =>
                {
                    x.User = user;
                    x.UserId = user.Id;
                    
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