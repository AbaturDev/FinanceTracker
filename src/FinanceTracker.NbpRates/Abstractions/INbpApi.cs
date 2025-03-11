using FinanceTracker.NbpRates.Dtos;

namespace FinanceTracker.NbpRates.Abstractions;

public interface INbpApi
{
    Task<ExchangeRateResponse?> GetExchangeRateAsync(RequestExchangeRate requestExchangeRate);
}