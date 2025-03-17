using FinanceTracker.Domain.Common;
using FinanceTracker.Domain.Entities.Owned;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceTracker.Domain.Entities;

public sealed record UserMonthlyBudget : BaseEntity
{
    public DateOnly Date { get; set; }
    public decimal TotalBudget { get; set; }
    public ExchangeRate? OriginalExchangeRate { get; set; }
    public required Guid UserId { get; set; }

    public required User User { get; set; }
    public ICollection<Income>? Incomes { get; set; }
}

public class UserMonthlyBudgetConfiguration : BaseEntityConfiguration<UserMonthlyBudget>
{
    public override void Configure(EntityTypeBuilder<UserMonthlyBudget> builder)
    {
        base.Configure(builder);

        builder.OwnsOne(x => x.OriginalExchangeRate)
            .Property(e => e.Mid)
            .HasPrecision(18, 2);

        builder.Property(x => x.TotalBudget)
            .HasPrecision(18, 2);

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Incomes)
            .WithMany(x => x.UserMonthlyBudgets);
    }
}