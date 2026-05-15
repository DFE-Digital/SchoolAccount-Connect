using Microsoft.Extensions.DependencyInjection;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Kernel.Conditions.Extensions;

namespace SchoolAccount.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.Scan(scan =>
            scan.FromAssembliesOf(typeof(DependencyInjection))
                .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)), publicOnly: false)
                .AsImplementedInterfaces()
                .WithScopedLifetime()
                .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>)), publicOnly: false)
                .AsImplementedInterfaces()
                .WithScopedLifetime()
                .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)), publicOnly: false)
                .AsImplementedInterfaces()
                .WithScopedLifetime()
        );

        services.AddConditions();

        //services.Decorate(typeof(ICommandHandler<,>), typeof(ValidationDecorator.CommandHandler<,>));
        //services.Decorate(typeof(ICommandHandler<,>), typeof(LoggingDecorator.CommandHandler<,>));

        return services;
    }
}
