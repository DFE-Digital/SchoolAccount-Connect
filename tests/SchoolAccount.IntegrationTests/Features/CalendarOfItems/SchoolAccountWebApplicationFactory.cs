using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Features.CalendarOfItems.Contracts;
using SchoolAccount.Application.Features.CalendarOfItems.Query;
using SchoolAccount.Infrastructure;
using SchoolAccount.IntegrationTests.Extensions;
using SchoolAccount.IntegrationTests.Fakes;
using SchoolAccount.IntegrationTests.Features.CalendarOfItems.Handlers;

namespace SchoolAccount.IntegrationTests.Features.CalendarOfItems;

public class SchoolAccountWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup>
    where TStartup : class
{
    public TestCalendarOfItemsDirectionalQueryHandler TestCalendarOfItemsDirectionalQueryHandler { get; } = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("IntegrationTest");

        builder.ConfigureTestServices(services =>
        {
            services.AddTransient<IPolicyEvaluator, FakePolicyEvaluator>();
            services.AddTransient<IApplicationDbContext, ApplicationDbContext>();

            services.ReplaceWithTransient<IQueryHandler<CalendarOfItemsDirectionalQuery, CalendarOfItemsPagedResult>>(
                _ => TestCalendarOfItemsDirectionalQueryHandler
            );
        });
    }
}
