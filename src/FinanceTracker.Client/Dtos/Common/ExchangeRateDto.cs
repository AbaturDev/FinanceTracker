namespace FinanceTracker.Client.Dtos.Common;

public sealed record ExchangeRateDto
{
    public required string CurrencyCode { get; set; }
    public decimal Mid { get; set; }
}