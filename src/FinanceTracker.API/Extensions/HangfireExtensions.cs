using FinanceTracker.Domain.Interfaces;
using FinanceTracker.Infrastructure.Context;
using Hangfire;

namespace FinanceTracker.API.Extensions;

public static class HangfireExtensions
{
    public static IServiceCollection AddHangfire(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHangfire(config =>
        {
            config.UseSimpleAssemblyNameTypeSerializer()
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection"));
        });
        
        services.AddHangfireServer();
        services.AddScoped<IRecurringJobManager, RecurringJobManager>();
        
        return services;
    }
    
    public static IApplicationBuilder UseHangfire(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var database = scope.ServiceProvider.GetRequiredService<FinanceTrackerDbContext>();
        var jobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();

        var users = database.Users.ToList();

        foreach (var user in users)
        {
            var jobId = $"generate-monthly-budget-{user.Id}";

            jobManager.AddOrUpdate<IUserMonthlyBudgetService>(
                jobId,
                service => service.GenerateMonthlyBudgetAsync(user.Id, CancellationToken.None),
                Cron.Monthly(1));
        }
        
        return app;
    }
}