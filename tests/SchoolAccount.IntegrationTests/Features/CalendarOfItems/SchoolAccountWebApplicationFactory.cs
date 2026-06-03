using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.Extensions.DependencyInjection;
using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Features.CalendarOfItems.Query.Operational;
using SchoolAccount.Infrastructure;
using SchoolAccount.IntegrationTests.Features.CalendarOfItems.Handlers;
using SchoolAccount.Tests.Common;
using SchoolAccount.Tests.Common.Extensions;
using SchoolAccount.Tests.Common.Fakes;

namespace SchoolAccount.IntegrationTests.Features.CalendarOfItems;

public class SchoolAccountWebApplicationFactory<TStartup> : SchoolAccountBaseWebApplicationFactory<TStartup>
    where TStartup : class
{
    public TestGetCalendarOfItemsOfSubTasksByDirectionForTabViewHandler TestGetCalendarOfItemsOfSubTasksByDirectionForTabViewHandler { get; } = new();

    protected override void ConfigureTestServices(IServiceCollection services)
    {
        services.ReplaceWithSingleton<IFallbackProviderResolver>(_ => FallbackProviderResolver);

        services.AddTransient<IPolicyEvaluator, FakePolicyEvaluator>();
        services.AddTransient<IApplicationDbContext, ApplicationDbContext>();

        services.ReplaceWithTransient<IQueryHandler<CalendarOfItemsDirectionalQuery, QueryPagedResult>>(_ =>
            TestGetCalendarOfItemsOfSubTasksByDirectionForTabViewHandler
        );
    }
}
