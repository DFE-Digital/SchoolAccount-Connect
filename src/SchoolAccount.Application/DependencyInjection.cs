using Microsoft.Extensions.DependencyInjection;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Features.Calendars.CalendarOfItems.Contracts;
using SchoolAccount.Application.Features.Calendars.CalendarOfItems.Query;
using SchoolAccount.Application.Features.Calendars.CalendarOfItems.Query.Operational;
using SchoolAccount.Integration.DfESignIn.Interfaces;

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
                .AddClasses(classes => classes.AssignableTo(typeof(IQueryPipelineBehavior<,>)), publicOnly: false)
                .AsImplementedInterfaces()
                .WithScopedLifetime()
                .AddClasses(classes => classes.AssignableTo(typeof(ICommandPipelineBehavior<,>)), publicOnly: false)
                .AsImplementedInterfaces()
                .WithScopedLifetime()
                .AddClasses(classes => classes.AssignableTo(typeof(ICommandPipelineBehavior<>)), publicOnly: false)
                .AsImplementedInterfaces()
                .WithScopedLifetime()
                .AddClasses(classes => classes.AssignableTo<IProvider>(), publicOnly: false)
                .AsImplementedInterfaces()
                .WithScopedLifetime()
                .AddClasses(classes => classes.AssignableTo<IProviderContextResolver>(), publicOnly: false)
                .AsImplementedInterfaces()
                .WithScopedLifetime()
        );

        services.AddScoped<IMediator, Mediator>();

        // CalendarOfItems uses abstract base queries whose concrete subtypes carry preset parameters.
        // Generic interfaces are invariant, so the base handler must be registered for each concrete query type.
        services.AddScoped<
            IQueryHandler<GetSubTasksByDirectionForTabViewCalendarOfItemsQuery, CalendarOfItemsResponse>,
            CalendarOfItemsDirectionalQueryHandler
        >();

        return services;
    }
}
