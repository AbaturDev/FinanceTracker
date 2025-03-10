using FinanceTracker.Domain.Common;
using FinanceTracker.Domain.Entities.Owned;
using FinanceTracker.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceTracker.Domain.Entities;

public sealed record Transaction : BaseEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public ExchangeRate? ExchangeRate { get; set; }
    public TransactionSource TransactionSource { get; set; }
    public required int UserId { get; set; }
    public int? UserMonthlyBudgetId { get; set; }
    public int? SavingGoalId { get; set; }
    public int? ExpensesPlannerId { get; set; }

    public required User User { get; set; }
    public UserMonthlyBudget? UserMonthlyBudget { get; set; }
    public SavingGoal? SavingGoal { get; set; }
    public ExpensesPlanner? ExpensesPlanner { get; set; }
}

public class TransactionConfiguration : BaseEntityConfiguration<Transaction>
{
    public override void Configure(EntityTypeBuilder<Transaction> builder)
    {
        base.Configure(builder);

        builder.OwnsOne(x => x.ExchangeRate);
        
        builder.Property(x => x.Amount)
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
            .WithMany()
            .HasForeignKey(t => t.SavingGoalId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.ExpensesPlanner)
            .WithMany(x => x.Transactions)
            .HasForeignKey(t => t.ExpensesPlannerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}