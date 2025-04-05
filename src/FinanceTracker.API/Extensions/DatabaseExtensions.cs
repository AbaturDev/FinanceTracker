using FinanceTracker.Domain.Entities;
using FinanceTracker.Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.API.Extensions;

public static class DatabaseExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<FinanceTrackerDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
        );

        services.AddIdentity<User, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<FinanceTrackerDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }
}