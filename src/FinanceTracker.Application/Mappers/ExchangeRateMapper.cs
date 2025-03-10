using FinanceTracker.Domain.Entities.Owned;
using FinanceTracker.Domain.Dtos.Owned;

namespace FinanceTracker.Application.Mappers;

public static class ExchangeRateMapper
{
    public static ExchangeRateDto? MapToExchangeRateDto(ExchangeRate? exchangeRate)
    {
        if (exchangeRate == null)
        {
            return null;
        }

        var exchangeRateDto = new ExchangeRateDto
        {
            CurrencyCode = exchangeRate.CurrencyCode,
            Mid = exchangeRate.Mid,
            Date = exchangeRate.Date,
        };
        
        return exchangeRateDto;
    }
}