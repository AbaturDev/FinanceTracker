namespace FinanceTracker.Domain.Dtos.Owned;

public sealed record ExchangeRateDto
{
    public string? CurrencyCode { get; set; }
    public decimal Mid { get; set; }
    public DateOnly Date { get; set; }
}