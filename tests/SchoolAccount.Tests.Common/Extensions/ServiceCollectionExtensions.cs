using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace SchoolAccount.Tests.Common.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ReplaceWithTransient<TService, TImplementation>(this IServiceCollection services)
        where TImplementation : class, TService
        where TService : class
    {
        services.RemoveAll<TService>();
        services.AddTransient<TService, TImplementation>();
        return services;
    }

    public static IServiceCollection ReplaceWithTransient<TService>(
        this IServiceCollection services,
        Func<IServiceProvider, TService> factory
    )
        where TService : class
    {
        services.RemoveAll<TService>();
        services.AddTransient(factory);
        return services;
    }

    public static IServiceCollection ReplaceWithSingleton<TService>(
        this IServiceCollection services,
        Func<IServiceProvider, TService> factory
    )
        where TService : class
    {
        services.RemoveAll<TService>();
        services.AddSingleton(factory);
        return services;
    }

    public static IServiceCollection ReplaceWithSingleton<TService, TImplementation>(this IServiceCollection services)
        where TImplementation : class, TService
        where TService : class
    {
        services.RemoveAll<TService>();
        services.AddSingleton<TService, TImplementation>();
        return services;
    }

    public static IServiceCollection RemoveByType(this IServiceCollection services, params Type[] descriptors)
    {
        foreach (var d in services.Where(x => descriptors.Any(y => x.ServiceType == y)))
        {
            services.Remove(d);
        }

        return services;
    }

    public static void ReplaceWithInMemory<TInterface, TContext>(this IServiceCollection services)
        where TInterface : class
        where TContext : DbContext, TInterface
    {
        services.RemoveByType(typeof(DbContextOptions<TContext>), typeof(TContext), typeof(TInterface));
        services.AddDbContext<TContext>(o => o.UseInMemoryDatabase(typeof(TContext).Name));
        services.AddTransient<TInterface>(sp => sp.GetRequiredService<TContext>());
    }
}
