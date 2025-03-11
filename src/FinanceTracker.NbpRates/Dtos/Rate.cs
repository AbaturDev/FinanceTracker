using System.Text.Json.Serialization;

namespace FinanceTracker.NbpRates.Dtos;

public sealed record Rate
{
    [JsonPropertyName("no")]
    public required string No { get; set; }
    
    [JsonPropertyName("effectiveDate")]
    public DateOnly EffectiveDate { get; set; }
    
    [JsonPropertyName("mid")]
    public decimal Mid { get; set; }
}