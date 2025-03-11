using System.Text.Json.Serialization;

namespace FinanceTracker.NbpRates.Dtos;

public sealed record ExchangeRateResponse
{
    [JsonPropertyName("table")]
    public required string Table { get; set; }
    
    [JsonPropertyName("currency")]
    public required string Currency { get; set; }
    
    [JsonPropertyName("code")]
    public required string Code { get; set; }
    
    [JsonPropertyName("rates")]
    public required Rate[] Rates { get; set; }
}