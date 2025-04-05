using FinanceTracker.Domain.Common;
using FinanceTracker.Domain.Entities.Owned;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceTracker.Domain.Entities;

public sealed record Transaction : BaseEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public decimal OriginalAmount { get; set; }
    public decimal CalculatedAmount { get; set; }
    public ExchangeRate? BudgetExchangeRate { get; set; }
    public ExchangeRate? TargetExchangeRate { get; set; }
    public required Guid UserId { get; set; }
    public int? UserMonthlyBudgetId { get; set; }
    public int? SavingGoalId { get; set; }
    public int? ExpensesPlannerId { get; set; }

    public User? User { get; set; }
    public UserMonthlyBudget? UserMonthlyBudget { get; set; }
    public SavingGoal? SavingGoal { get; set; }
    public ExpensesPlanner? ExpensesPlanner { get; set; }
}

public class TransactionConfiguration : BaseEntityConfiguration<Transaction>
{
    public override void Configure(EntityTypeBuilder<Transaction> builder)
    {
        base.Configure(builder);

        builder.OwnsOne(x => x.BudgetExchangeRate)
            .Property(e => e.Mid)
            .HasPrecision(18, 2);

        builder.OwnsOne(x => x.TargetExchangeRate)
            .Property(e => e.Mid)
            .HasPrecision(18, 2);

        builder.Property(x => x.OriginalAmount)
            .HasPrecision(18, 2);

        builder.Property(x => x.CalculatedAmount)
            .HasPrecision(18, 2);

        builder.HasOne(t => t.User)
            .WithMany()
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.UserMonthlyBudget)
            .WithMany()
            .HasForeignKey(t => t.UserMonthlyBudgetId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.SavingGoal)
            .WithMany(x => x.Transactions)
            .HasForeignKey(t => t.SavingGoalId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.ExpensesPlanner)
            .WithMany(x => x.Transactions)
            .HasForeignKey(t => t.ExpensesPlannerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}