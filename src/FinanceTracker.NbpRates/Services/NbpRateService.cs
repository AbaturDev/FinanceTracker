using FinanceTracker.NbpRates.Abstractions;
using FinanceTracker.NbpRates.Dtos;
using FinanceTracker.NbpRates.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace FinanceTracker.NbpRates.Services;

public class NbpRateService : INbpRateService
{
    private static readonly MemoryCacheEntryOptions CacheOptions = new MemoryCacheEntryOptions()
        .SetSlidingExpiration(TimeSpan.FromHours(12))
        .SetAbsoluteExpiration(TimeSpan.FromDays(1))
        .SetPriority(CacheItemPriority.Normal);
    
    private const int MaxPreviousDaysAttempts = 5;
    
    private readonly IMemoryCache _memoryCache;
    private readonly INbpApi _nbpApi;
    
    public NbpRateService(IMemoryCache memoryCache, INbpApi nbpApi)
    {
        _memoryCache = memoryCache;
        _nbpApi = nbpApi;
    }
    
    public async Task<NbpExchangeRateDto?> GetExchangeRateAsync(string currencyCode)
    {
        var requestDate = DateOnly.FromDateTime(DateTime.UtcNow);
        
        if (currencyCode == "PLN")
        {
            return new NbpExchangeRateDto
            {
                Date = requestDate,
                Mid = 1.000m,
            };
        }

        if (_memoryCache.TryGetValue(GetNbpMemoryCacheKey(currencyCode, requestDate), out var cacheValue) && cacheValue is NbpExchangeRateDto cachedRate)
        {
            return new NbpExchangeRateDto
            {
                Date = cachedRate.Date,
                Mid = cachedRate.Mid,
            };
        }

        var request = new RequestExchangeRate
        {
            CurrencyCode = currencyCode,
            StartDate = requestDate.AddDays(-MaxPreviousDaysAttempts),
            EndDate = requestDate,
        };

        var response = await _nbpApi.GetExchangeRateAsync(request);

        if (response.Rates.Length <= 0)
        {
            return null;
        }
        
        var lastRate = response.Rates.Last();
        
        var nbpExchangeRateDto = new NbpExchangeRateDto
        {
            Date = lastRate.EffectiveDate,
            Mid = lastRate.Mid,
        };
        
        _memoryCache.Set(GetNbpMemoryCacheKey(currencyCode, requestDate), nbpExchangeRateDto, CacheOptions);
        
        return nbpExchangeRateDto;
    }

    private static string GetNbpMemoryCacheKey(string currencyCode, DateOnly date)
    {
        return "NbpMemoryCacheKey_" + currencyCode + "_" + date;
    }
}