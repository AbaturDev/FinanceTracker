namespace FinanceTracker.Client.Interfaces;

public interface IAddBearerTokenService
{
    Task AddBearerTokenAsync(HttpClient httpClient, CancellationToken ct);
}