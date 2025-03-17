using FinanceTracker.Application.Common;
using FinanceTracker.Application.Services;
using FinanceTracker.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FinanceTracker.Application;

public static class Setup
{
    public static void AddApplicationLogic(this IHostApplicationBuilder builder, IConfiguration configuration)
    {
        var jwtOptions = configuration.GetSection("JwtSettings").Get<JwtOptions>();

        builder.Services.AddSingleton(jwtOptions);

        builder.Services.AddScoped<IAccountService, AccountService>();
        builder.Services.AddScoped<IUserContextService, UserContextService>();
    }
}