using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace SchoolAccount.IntegrationTests.Extensions;

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
}
