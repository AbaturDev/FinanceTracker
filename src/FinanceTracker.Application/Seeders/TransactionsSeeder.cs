using Bogus;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Entities.Owned;
using FinanceTracker.Infrastructure.Context;

namespace FinanceTracker.Application.Seeders
{
    public class TransactionsSeeder
    {
        public static void Seed(FinanceTrackerDbContext dbContext, int transactionsCount)
        {
            if (dbContext.UserMonthlyBudgets.Any())
            {
                return;
            }

            var sampleUsers = dbContext.Users.ToList();
            var sampleUserMonthlyBudgets = dbContext.UserMonthlyBudgets.ToList();
            var sampleSavingGoals = dbContext.SavingGoals.ToList();
            var sampleExpensesPlanners = dbContext.ExpensesPlanners.ToList();

            var faker = new Faker<Transaction>()
                .RuleFor(t => t.Name, f => f.Name.FirstName())
                .RuleFor(t => t.Description, f => f.Lorem.Sentence())
                .RuleFor(t => t.OriginalAmount, f => f.Finance.Amount())
                .RuleFor(t => t.CalculatedAmount, f => f.Finance.Amount())
                // .RuleFor(t => t.BudgetExchangeRate)
                // .RuleFor(t => t.TargetExchangeRate)
                .RuleFor(t => t.UserId, f => f.PickRandom(sampleUsers).Id)
                .RuleFor(t => t.UserMonthlyBudgetId, f => f.PickRandom(sampleUserMonthlyBudgets).Id)
                .RuleFor(t => t.SavingGoalId, f => f.PickRandom(sampleSavingGoals).Id)
                .RuleFor(t => t.ExpensesPlannerId, f => f.PickRandom(sampleExpensesPlanners).Id)
                .RuleFor(t => t.User, f => f.PickRandom(sampleUsers))
                .RuleFor(t => t.UserMonthlyBudget, f => f.PickRandom(sampleUserMonthlyBudgets))
                .RuleFor(t => t.SavingGoal, f => f.PickRandom(sampleSavingGoals))
                .RuleFor(t => t.ExpensesPlanner, f => f.PickRandom(sampleExpensesPlanners));

            var transactions = faker.Generate(transactionsCount);

            dbContext.Transactions.AddRange(transactions);
            dbContext.SaveChanges();
        }
    }
}