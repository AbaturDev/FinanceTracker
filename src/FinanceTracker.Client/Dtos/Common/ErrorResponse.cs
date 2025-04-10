using System.Text.Json.Serialization;

namespace FinanceTracker.Client.Dtos.Common;

public sealed record ErrorResponse
{
    [JsonPropertyName("reasons")]
    public string[] Reasons { get; set; }
    [JsonPropertyName("message")]
    public string Message { get; set; }
    [JsonPropertyName("metadata")]
    public object Metadata { get; set; }
}