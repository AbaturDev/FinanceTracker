namespace FinanceTracker.NbpRates.Dtos;

public record RequestExchangeRate
{
    public required string CurrencyCode { get; set; }
}