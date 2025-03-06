using FinanceTracker.Domain.Common;
using FinanceTracker.Domain.Entities.Owned;
using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Domain.Entities;

public sealed record ExpensesPlanner : BaseEntity
{
    public required string Name { get; set; }
    public decimal Budget { get; set; }
    public string? OriginalCurrencyCode { get; set; }
    public ExchangeRate? OriginalExchangeRate { get; set; }
    public ResetInterval ResetInterval { get; set; }
    public required int UserId { get; set; }

    public ICollection<Category>? Categories { get; set; }
    public required User User { get; set; }
}