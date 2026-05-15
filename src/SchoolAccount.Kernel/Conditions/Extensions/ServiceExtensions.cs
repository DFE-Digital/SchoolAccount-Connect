using Microsoft.Extensions.DependencyInjection;
using SchoolAccount.Kernel.Conditions.Interface;
using SchoolAccount.Kernel.Conditions.Resolvers;

namespace SchoolAccount.Kernel.Conditions.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddConditions(this IServiceCollection services)
    {
        services.Scan(scan =>
            scan.FromAssembliesOf(typeof(IConditionMapper))
                .AddClasses(classes => classes.AssignableTo<IConditionMapper>(), publicOnly: false)
                .AsImplementedInterfaces()
                .WithScopedLifetime()
        );

        services.AddScoped<IConditionMapperResolver, ConditionMapperResolver>();
        
        return services;
    }
}