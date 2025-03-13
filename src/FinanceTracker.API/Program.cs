using FinanceTracker.Infrastructure.Context;
using FinanceTracker.NbpRates;
using FinanceTracker.NbpRates.Abstractions;
using FinanceTracker.NbpRates.Dtos;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<FinanceTrackerDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.AddNbpIntegration();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment()) app.MapOpenApi();

app.UseHttpsRedirection();

app.MapGet("/weatherforecast", async (INbpApi nbpApi) =>
    {
        var end = DateOnly.FromDateTime(DateTime.Now);
        var start = DateOnly.FromDateTime(DateTime.Now.AddDays(-3));

        var request = new RequestExchangeRate()
        {
            CurrencyCode = "USD",
            StartDate = start,
            EndDate = end,
        };
        
        var test = await nbpApi.GetExchangeRateAsync(request);
        
        return test;
    })
    .WithName("GetWeatherForecast");

app.Run();
