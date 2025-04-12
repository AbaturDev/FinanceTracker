using System.Net.Http.Json;
using FinanceTracker.Client.Dtos.Common;

namespace FinanceTracker.Client.Utils;

public static class GetErrorMessageUtils
{
    public static async Task<string?> GetErrorMessage(HttpResponseMessage response, CancellationToken ct)
    {
        var errorDetails = await response.Content.ReadFromJsonAsync<List<ErrorResponse>>(cancellationToken: ct);

        var errorMessage = errorDetails?[0].Message;

        return errorMessage;
    }
}