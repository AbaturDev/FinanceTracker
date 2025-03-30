using FinanceTracker.Domain.Common;
using FinanceTracker.Domain.Entities.Owned;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceTracker.Domain.Entities;

public sealed record Income : BaseEntity
{
    public required string Name { get; set; }
    public decimal Amount { get; set; }
    public bool RegularIncome { get; set; }
    public bool IsActiveThisMonth { get; set; }
    public ExchangeRate? OriginalExchangeRate { get; set; }
    public required Guid UserId { get; set; }

    public User? User { get; set; }
    public ICollection<UserMonthlyBudget>? UserMonthlyBudgets { get; set; }
}

public class IncomeConfiguration : BaseEntityConfiguration<Income>
{
    public override void Configure(EntityTypeBuilder<Income> builder)
    {
        base.Configure(builder);

        builder.OwnsOne(x => x.OriginalExchangeRate)
            .Property(e => e.Mid)
            .HasPrecision(18, 2);
        
        builder.Property(x => x.Amount)
            .HasPrecision(18, 2);

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.UserMonthlyBudgets)
            .WithMany(i => i.Incomes);
    }
}