using System.Diagnostics.CodeAnalysis;
using MartinCostello.Logging.XUnit;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Infrastructure;
using SchoolAccount.Tests.Common.Extensions;
using SchoolAccount.Tests.Common.Fakes;
using Xunit;

namespace SchoolAccount.Tests.Common.Factories;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.AllConstructors)]
public class SchoolAccountWebApplicationFactory(
    TestQueryHandlerRegistry? handlerRegistry = null,
    StubFallbackProviderResolver? fallbackProviderResolver = null,
    bool useSessionAuthentication = false,
    bool useFakePolicyEvaluator = true
) : WebApplicationFactory<Program>, ITestOutputHelperAccessor
{
    private string? _baseUrl;

    public TestQueryHandlerRegistry HandlerRegistry { get; } = handlerRegistry ?? new TestQueryHandlerRegistry();

    public ITestOutputHelper? OutputHelper { get; set; }

    public StubFallbackProviderResolver FallbackProviderResolver { get; } =
        fallbackProviderResolver ?? new StubFallbackProviderResolver();

    public string StartKestrel()
    {
        if (_baseUrl is not null)
            return _baseUrl;

        UseKestrel(port: 0);
        StartServer();

        var addresses = Services.GetRequiredService<IServer>().Features.Get<IServerAddressesFeature>()!.Addresses;

        _baseUrl = addresses.First();
        return _baseUrl;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("IntegrationTest");
        builder.ConfigureLogging(p => p.AddXUnit(this));
        builder.ConfigureTestServices(ConfigureTestServices);
    }

    private void ConfigureTestServices(IServiceCollection services)
    {
        services.ReplaceWithInMemory<IApplicationDbContext, ApplicationDbContext>();
        services.ReplaceWithSingleton<IFallbackProviderResolver>(_ => FallbackProviderResolver);
        services.ReplaceWithSingleton<IAntiforgery, DisabledAntiforgery>();
        services.AddDistributedMemoryCache();

        if (useSessionAuthentication)
        {
            services
                .AddAuthentication(SessionAuthenticationHandler.SchemeName)
                .AddScheme<AuthenticationSchemeOptions, SessionAuthenticationHandler>(
                    SessionAuthenticationHandler.SchemeName,
                    _ => { }
                );
        }

        if (useFakePolicyEvaluator)
        {
            services.AddTransient<IPolicyEvaluator, FakePolicyEvaluator>();
        }

        services.AddTransient<IApplicationDbContext, ApplicationDbContext>();
        services.AddSingleton(HandlerRegistry);

        ReplaceHandlersWithTestAdapters(services);
    }

    private void ReplaceHandlersWithTestAdapters(IServiceCollection services)
    {
        foreach (var serviceType in HandlerRegistry.ServiceTypes)
        {
            var existing = services.FirstOrDefault(d => d.ServiceType == serviceType);
            if (existing is not null)
                services.Remove(existing);

            var typeArgs = serviceType.GetGenericArguments();
            var adapterType = typeof(TestQueryHandlerAdapter<,>).MakeGenericType(typeArgs);
            services.AddTransient(serviceType, adapterType);
        }
    }
}
