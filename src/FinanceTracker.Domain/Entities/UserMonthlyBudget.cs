using FinanceTracker.Domain.Common;
using FinanceTracker.Domain.Entities.Owned;

namespace FinanceTracker.Domain.Entities;

public sealed record UserMonthlyBudget : BaseEntity
{
    public DateOnly Date { get; set; }
    public decimal TotalBudget { get; set; }
    public string? OriginalCurrencyCode { get; set; }
    public ExchangeRate? OriginalExchangeRate { get; set; }
    public required int UserId { get; set; }

    public required User User { get; set; }
}