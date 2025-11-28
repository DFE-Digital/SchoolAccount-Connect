using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SchoolAccount.Application.Abstractions.Data;

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
        var connectionString = configuration.GetConnectionString("SchoolAccount");

        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString, nameof(connectionString));

        services
            .AddDbContext<IApplicationDbContext, ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

        return services;
    }

    private static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("SchoolAccount");

        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

        services
            .AddHealthChecks()
            .AddSqlServer(
                connectionString: connectionString,
                healthQuery: "SELECT 1;",
                name: "sql",
                failureStatus: HealthStatus.Degraded,
                tags: ["db", "sql", "sqlserver"]);

        return services;
    }
}