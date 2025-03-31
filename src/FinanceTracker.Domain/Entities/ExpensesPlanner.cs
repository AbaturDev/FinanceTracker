using FinanceTracker.Domain.Common;
using FinanceTracker.Domain.Entities.Owned;
using FinanceTracker.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceTracker.Domain.Entities;

public sealed record ExpensesPlanner : BaseEntity
{
    public required string Name { get; set; }
    public decimal Budget { get; set; }
    public decimal SpentAmount { get; set; }
    public string? CurrencyCode { get; set; }
    public Category? Category { get; set; }
    public ResetInterval ResetInterval { get; set; }
    public required Guid UserId { get; set; }

    public User? User { get; set; }
    public ICollection<Transaction>? Transactions { get; set; }
}

public class ExpensesPlannerConfiguration : BaseEntityConfiguration<ExpensesPlanner>
{
    public override void Configure(EntityTypeBuilder<ExpensesPlanner> builder)
    {
        base.Configure(builder);
        
        builder.OwnsOne(x => x.Category);

        builder.Property(x => x.Budget)
            .HasPrecision(18, 2);

        builder.Property(x => x.SpentAmount)
            .HasPrecision(18, 2);

        builder.HasOne(u => u.User)
            .WithMany()
            .HasForeignKey(u => u.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Transactions)
            .WithOne(t => t.ExpensesPlanner)
            .HasForeignKey(t => t.ExpensesPlannerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}