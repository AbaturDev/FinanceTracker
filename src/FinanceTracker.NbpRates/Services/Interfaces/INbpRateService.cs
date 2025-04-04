using FinanceTracker.NbpRates.Dtos;

namespace FinanceTracker.NbpRates.Services.Interfaces;

public interface INbpRateService
{
    Task<NbpExchangeRateDto?> GetExchangeRateAsync(string currencyCode);
}