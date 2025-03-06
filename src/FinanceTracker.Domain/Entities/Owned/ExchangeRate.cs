namespace FinanceTracker.Domain.Entities.Owned;

public sealed record ExchangeRate
{
    public DateOnly Date { get; set; }
    public decimal Mid { get; set; }
}