using System.Text.Json.Serialization;

namespace FinanceTracker.NbpRates.Dtos;

public sealed record ExchangeRateResponse
{
    public required string Table { get; set; }
    public required string Currency { get; set; }
    public required string Code { get; set; }
    public required Rate[] Rates { get; set; }
}