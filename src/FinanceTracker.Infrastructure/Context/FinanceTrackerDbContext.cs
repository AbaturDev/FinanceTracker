using FinanceTracker.Domain.Common;
using FinanceTracker.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Infrastructure.Context;

public class FinanceTrackerDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public FinanceTrackerDbContext(DbContextOptions<FinanceTrackerDbContext> options) : base(options)
    {
    }

    public DbSet<ExpensesPlanner> ExpensesPlanners { get; set; }
    public DbSet<Income> Incomes { get; set; }
    public DbSet<SavingGoal> SavingGoals { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<UserMonthlyBudget> UserMonthlyBudgets { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(BaseEntityConfiguration<>).Assembly);
    }
}