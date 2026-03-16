using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Infrastructure.Mapping;
using SchoolAccount.Infrastructure.Repository;
using SchoolAccount.Infrastructure.Time;
using SchoolAccount.Kernel;

namespace SchoolAccount.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabase(configuration).AddHealthChecks(configuration).AddMappers().AddServices();

        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("SchoolAccount");

        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

        services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(options =>
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
}
