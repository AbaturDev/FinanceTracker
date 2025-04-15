using System.Net.Http.Json;
using System.Text.Json;
using FinanceTracker.Client.Dtos.Common;

namespace FinanceTracker.Client.Utils;

public static class GetErrorMessageUtils
{
    public static async Task<string?> GetErrorMessage(HttpResponseMessage response, CancellationToken ct)
    {
        var content = await response.Content.ReadAsStringAsync(ct);

        
        try
        {
            var errorDetails = JsonSerializer.Deserialize<List<ErrorResponse>>(content);
            
            var errorMessage = errorDetails?[0].Message;

            return errorMessage;
        }
        catch { /* ignore */ }

        try
        {
            var errorDetails = JsonSerializer.Deserialize<FluentValidationResponse>(content);
            if (errorDetails?.Errors is not null)
            {
                if (errorDetails?.Errors is not null)
                {
                    return string.Join("\n", errorDetails.Errors.SelectMany(kvp => kvp.Value));
                }
            }
        }
        catch { /* ignore */ }
        
        return !string.IsNullOrWhiteSpace(content)
            ? content
            : $"Unexpected error (status code {response.StatusCode})";
    }
}