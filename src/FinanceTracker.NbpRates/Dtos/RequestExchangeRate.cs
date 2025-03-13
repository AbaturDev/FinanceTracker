using FinanceTracker.NbpRates.Constants;

namespace FinanceTracker.NbpRates.Dtos;

public record RequestExchangeRate
{
    public required string CurrencyCode { get; set; }
    public string CurrencyTableType { get; set; } = NbpTableTypes.ForeignCurrencyAverageRate;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
}