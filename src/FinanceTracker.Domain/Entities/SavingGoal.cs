using FinanceTracker.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceTracker.Domain.Entities;

public sealed record SavingGoal : BaseEntity
{
    public required string Name { get; set; }
    public decimal CurrentBalance { get; set; }
    public decimal AmountOfMoney { get; set; }
    public required string Goal { get; set; }
    public DateOnly? DueDate { get; set; }
    public required string CurrencyCode { get; set; }
    public required Guid UserId { get; set; }

    public required User User { get; set; }
    public ICollection<Transaction>? Transactions { get; set; }
}

public class SavingGoalConfiguration : BaseEntityConfiguration<SavingGoal>
{
    public override void Configure(EntityTypeBuilder<SavingGoal> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.CurrentBalance)
            .HasPrecision(18, 2);

        builder.Property(x => x.AmountOfMoney)
            .HasPrecision(18, 2);

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Transactions)
            .WithOne(x => x.SavingGoal)
            .HasForeignKey(x => x.SavingGoalId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}