using System.Text.Json.Serialization;

namespace FinanceTracker.NbpRates.Dtos;

public sealed record Rate
{
    public required string No { get; set; }
    public DateOnly EffectiveDate { get; set; }
    public decimal Mid { get; set; }
}