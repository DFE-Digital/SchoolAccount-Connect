using System.Diagnostics.CodeAnalysis;
using MartinCostello.Logging.XUnit;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Infrastructure;
using SchoolAccount.IntegrationTests.Testing;
using SchoolAccount.Tests.Common.Extensions;
using SchoolAccount.Tests.Common.Fakes;
using Xunit;

namespace SchoolAccount.IntegrationTests;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.AllConstructors)]
public class SchoolAccountWebApplicationFactory : WebApplicationFactory<Program>, ITestOutputHelperAccessor
{
    public TestQueryHandlerRegistry HandlerRegistry { get; } = new();

    public ITestOutputHelper? OutputHelper { get; set; }

    public StubFallbackProviderResolver FallbackProviderResolver { get; } = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("IntegrationTest");
        builder.ConfigureLogging(p => p.AddXUnit(this));
        builder.ConfigureTestServices(ConfigureTestServices);
    }

    private void ConfigureTestServices(IServiceCollection services)
    {
        services.ReplaceWithInMemory<IApplicationDbContext, ApplicationDbContext>();
        services.ReplaceWithSingleton<IFallbackProviderResolver>(x => FallbackProviderResolver);
        services.ReplaceWithSingleton<IFallbackProviderResolver>(_ => FallbackProviderResolver);
        services.AddTransient<IPolicyEvaluator, FakePolicyEvaluator>();
        services.AddTransient<IApplicationDbContext, ApplicationDbContext>();

        var queryHandlerType = typeof(IQueryHandler<,>);
        var handlersToRemove = services
            .Where(d => d.ServiceType.IsGenericType && d.ServiceType.GetGenericTypeDefinition() == queryHandlerType)
            .ToList();
        foreach (var descriptor in handlersToRemove)
            services.Remove(descriptor);

        services.AddSingleton(HandlerRegistry);
        services.AddTransient(typeof(IQueryHandler<,>), typeof(TestQueryHandlerAdapter<,>));
    }
}
