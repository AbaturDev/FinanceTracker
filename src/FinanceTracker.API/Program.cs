using FinanceTracker.Application;
using FinanceTracker.Domain.Dtos.Account;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Interfaces;
using FinanceTracker.Infrastructure.Context;
using FinanceTracker.NbpRates;
using FinanceTracker.NbpRates.Abstractions;
using FinanceTracker.NbpRates.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<FinanceTrackerDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddIdentity<User, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<FinanceTrackerDbContext>()
    .AddDefaultTokenProviders();

builder.AddApplicationLogic(builder.Configuration);
builder.AddNbpIntegration();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment()) app.MapOpenApi();

app.UseHttpsRedirection();

app.MapGet("/login", async (IAccountService test, CancellationToken ct) =>
{
    var registetr = new RegisterDto
    {
        Email = "d",
        Password = "s",
        UserName = "abc",
        VerifyPassword = "s",
        CurrencyCode = "PLN"
    };

    await test.RegisterAsync(registetr, ct);
    
    var login = new LoginDto()
    {
        UserName = registetr.UserName,
        Password = registetr.Password
    };
    
    var token = await test.LoginAsync(login, ct);
    return token;
});

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
