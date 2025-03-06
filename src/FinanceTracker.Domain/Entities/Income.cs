using FinanceTracker.Domain.Common;
using FinanceTracker.Domain.Entities.Owned;

namespace FinanceTracker.Domain.Entities;

public sealed record Income : BaseEntity
{
    public required string Name { get; set; }
    public decimal Amount { get; set; }
    public bool RegularIncome { get; set; }
    public bool IsActiveThisMonth { get; set; }
    public string? OriginalCurrencyCode { get; set; }
    public ExchangeRate? OriginalExchangeRate { get; set; }
    public required int UserId { get; set; }

    public required User User { get; set; }
}