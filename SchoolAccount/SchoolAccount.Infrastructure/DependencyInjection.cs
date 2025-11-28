using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace SchoolAccount.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddDatabase(configuration)
            .AddHealthChecks(configuration);

        return services;
    }
    

    
    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .SetupEntity(configuration);

        return services;
    }

    private static void SetupEntity(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration["saSqlConnectionString"];

        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString, nameof(connectionString));

        services
            .AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
            {

                options
                    .UseSqlServer(connectionString);

            });

        services
            .AddScoped<IUnitOfWork, UnitOfWork>();
    }

    private static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration["saSqlConnectionString"];

        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString, nameof(connectionString));

        services
            .AddHealthChecks()
            .AddSqlServer(
                connectionString: configuration.GetConnectionString(connectionString)!,
                healthQuery: "SELECT 1;",
                name: "sql",
                failureStatus: HealthStatus.Degraded,
                tags: ["db", "sql", "sqlserver"]);

        return services;
    }
}