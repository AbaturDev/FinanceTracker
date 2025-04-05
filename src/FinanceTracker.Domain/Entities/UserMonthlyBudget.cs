using FinanceTracker.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceTracker.Domain.Entities;

public sealed record UserMonthlyBudget : BaseEntity
{
    public DateOnly Date { get; set; }
    public decimal TotalBudget { get; set; }
    public decimal TotalExpenses { get; set; }
    public required string CurrencyCode { get; set; }
    public required Guid UserId { get; set; }

    public User? User { get; set; }
    public ICollection<Income>? Incomes { get; set; }
}

public class UserMonthlyBudgetConfiguration : BaseEntityConfiguration<UserMonthlyBudget>
{
    public override void Configure(EntityTypeBuilder<UserMonthlyBudget> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.TotalBudget)
            .HasPrecision(18, 2);

        builder.Property(x => x.TotalExpenses)
            .HasPrecision(18, 2);

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Incomes)
            .WithMany(x => x.UserMonthlyBudgets);
    }
}