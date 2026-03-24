using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SchoolAccount.Application.Abstractions.Aggregators;
using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Infrastructure.Abstraction;
using SchoolAccount.Infrastructure.Aggregators;
using SchoolAccount.Infrastructure.Mapping;
using SchoolAccount.Infrastructure.Repository;
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
        services.AddMappers();
        services.AddServices();
        services.AddCalendarOfItemsEngine();

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

    private static IServiceCollection AddMappers(this IServiceCollection services)
    {
        services.Scan(scan =>
            scan.FromAssembliesOf(typeof(DependencyInjection))
                .AddClasses(classes => classes.AssignableTo(typeof(IDomainEntityToDatabaseEntityMapper<,>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime()
        );

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddScoped<IPageReadStore, PageReadRepository>();

        return services;
    }

    private static IServiceCollection AddCalendarOfItemsEngine(this IServiceCollection services)
    {
        services.Scan(scan =>
            scan.FromAssembliesOf(typeof(DependencyInjection))
                .AddClasses(classes => classes.AssignableTo<ICalendarOfItemsQueryFactory>())
                .AsImplementedInterfaces()
                .WithScopedLifetime()
        );

        services.AddScoped<ICalendarOfItemsAggregator, CalendarOfItemsAggregator>();
        services.AddScoped<CalendarOfItemsQueryFactoryResolver>();

        return services;
    }
}
