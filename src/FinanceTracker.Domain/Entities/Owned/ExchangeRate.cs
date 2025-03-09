namespace FinanceTracker.Domain.Entities.Owned;

public sealed record ExchangeRate
{
    public string? CurrencyCode { get; set; }
    public DateOnly Date { get; set; }
    public decimal Mid { get; set; }
}