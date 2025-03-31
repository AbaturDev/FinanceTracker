using FinanceTracker.NbpRates.Dtos;
using Refit;

namespace FinanceTracker.NbpRates.Abstractions;

public interface INbpApi
{
    [Get("/exchangerates/rates/{requestExchangeRate.CurrencyTableType}/{requestExchangeRate.CurrencyCode}/{requestExchangeRate.StartDate}/{requestExchangeRate.EndDate}/?format=json")]
    Task<ExchangeRateResponse> GetExchangeRateAsync(RequestExchangeRate requestExchangeRate);
}