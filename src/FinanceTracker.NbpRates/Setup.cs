using System.Text.Json;
using FinanceTracker.NbpRates.Abstractions;
using FinanceTracker.NbpRates.Services;
using FinanceTracker.NbpRates.Services.Interfaces;
using FinanceTracker.NbpRates.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Refit;

namespace FinanceTracker.NbpRates;

public static class Setup
{
    public static void AddNbpIntegration(this IHostApplicationBuilder builder)
    {
        builder.Services.AddRefitClient<INbpApi>(new()
            {
                ContentSerializer = new SystemTextJsonContentSerializer(new ()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                }),
                UrlParameterFormatter = new CustomDateOnlyParameterFormatterUtils(),
            })
            .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://api.nbp.pl/api"))
            .AddPolicyHandler(NbpPolicesHandlerUtils.GetNbpRetryPolicy());

        builder.Services.AddMemoryCache();
        builder.Services.AddScoped<INbpRateService, NbpRateService>();
    }
}