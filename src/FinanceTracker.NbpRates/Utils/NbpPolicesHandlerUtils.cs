using Polly;
using Polly.Extensions.Http;

namespace FinanceTracker.NbpRates.Utils;

public static class NbpPolicesHandlerUtils
{
    private const int MaxAttempts = 5;

    public static IAsyncPolicy<HttpResponseMessage> GetNbpRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound || msg.StatusCode == System.Net.HttpStatusCode.BadRequest)
            .WaitAndRetryAsync(MaxAttempts, retryAttempt => TimeSpan.FromSeconds(Math.Pow(MaxAttempts, retryAttempt)));
    }
}