using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Infrastructure.Resolvers;
using SchoolAccount.Infrastructure.Time;
using SchoolAccount.Kernel;

namespace SchoolAccount.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        ILogger logger
    )
    {
        services.AddDatabase(configuration, logger);
        services.AddHealthChecks(configuration, logger);
        services.AddServices();

        services.AddSingleton<IFallbackProviderResolver>(sp =>
        {
            using var scope = sp.CreateScope();
            var database = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
            var overrides = database.ProviderOverrides.AsNoTracking().ToList();
            return new FallbackProviderResolver(overrides);
        });

        return services;
    }

    private static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration,
        ILogger logger
    )
    {
        var connectionString = configuration.GetConnectionString("SchoolAccount");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            logger.LogWarning("No database connection string found. Skipping database setup.");
            return services;
        }

        services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        return services;
    }

    private static IServiceCollection AddHealthChecks(
        this IServiceCollection services,
        IConfiguration configuration,
        ILogger logger
    )
    {
        var connectionString = configuration.GetConnectionString("SchoolAccount");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            logger.LogWarning("No database connection string found. Skipping health checks setup.");
            return services;
        }

        var healthChecks = services.AddHealthChecks();

        healthChecks.AddSqlServer(
            connectionString: connectionString,
            healthQuery: "SELECT 1;",
            name: "sql",
            failureStatus: HealthStatus.Degraded,
            tags: ["db", "sql", "sqlserver"]
        );

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        return services;
    }
}
