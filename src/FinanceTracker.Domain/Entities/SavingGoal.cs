using FinanceTracker.Domain.Common;
using FinanceTracker.Domain.Entities.Owned;

namespace FinanceTracker.Domain.Entities;

public sealed record SavingGoal : BaseEntity
{
    public required string Name { get; set; }
    public decimal AmountOfMoney { get; set; }
    public required string Goal { get; set; }
    public DateOnly? DueDate { get; set; }
    public string? OriginalCurrencyCode { get; set; }
    public ExchangeRate? OriginalExchangeRate { get; set; }
    public required int UserId { get; set; }
    public required User User { get; set; }
}