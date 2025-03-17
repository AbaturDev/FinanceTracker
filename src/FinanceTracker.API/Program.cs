using System.Text;
using FinanceTracker.Application;
using FinanceTracker.Domain.Dtos.Account;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Interfaces;
using FinanceTracker.Infrastructure.Context;
using FinanceTracker.NbpRates;
using FinanceTracker.NbpRates.Abstractions;
using FinanceTracker.NbpRates.Dtos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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

builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Secret"]))
        };
    });
builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment()) app.MapOpenApi();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/register", async (IAccountService test, CancellationToken ct, [FromBody] RegisterDto dto) => {
    await test.RegisterAsync(dto, ct);
})
.AllowAnonymous();

app.MapGet("/login", async (IAccountService test, CancellationToken ct, [FromBody] LoginDto dto) =>
{
    var token = await test.LoginAsync(dto, ct);
    return token.Value;
})
.AllowAnonymous();

app.MapGet("/api/nbp", async (INbpApi nbpApi) =>
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
.RequireAuthorization();

app.Run();
