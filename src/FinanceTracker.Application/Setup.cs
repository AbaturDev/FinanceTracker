using FinanceTracker.Application.Common;
using FinanceTracker.Application.Services;
using FinanceTracker.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Protocols.Configuration;

namespace FinanceTracker.Application;

public static class Setup
{
    public static void AddApplicationLogic(this IHostApplicationBuilder builder, IConfiguration configuration)
    {
        var jwtOptions = configuration.GetSection("JwtSettings").Get<JwtOptions>();
        if (jwtOptions == null)
        {
            throw new InvalidConfigurationException("JWTSettings not found in configuration");
        }

        builder.Services.AddSingleton(jwtOptions);

        builder.Services.AddScoped<IAccountService, AccountService>();
        builder.Services.AddScoped<IUserContextService, UserContextService>();
        builder.Services.AddScoped<IExpensesPlannerService, ExpensesPlannerService>();
        builder.Services.AddScoped<IIncomeService, IncomeService>();
        builder.Services.AddScoped<IUserMonthlyBudgetService, UserMonthlyBudgetService>();
    }
}