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

public partial class SchoolAccountWebApplicationFactory : WebApplicationFactory<Program>, ITestOutputHelperAccessor
{
    private readonly bool _useSessionAuthentication;
    private readonly bool _useFakePolicyEvaluator;
    private readonly bool _useDisabledAntiforgery;
    private readonly bool _useInMemoryDatabase;
    private string? _baseUrl;

    private SchoolAccountWebApplicationFactory(Builder builder)
    {
        HandlerRegistry = builder.HandlerRegistry ?? new TestQueryHandlerRegistry();
        FallbackProviderResolver = builder.FallbackProviderResolver ?? new StubFallbackProviderResolver();
        _useSessionAuthentication = builder.UseSessionAuthentication;
        _useFakePolicyEvaluator = builder.UseFakePolicyEvaluator;
        _useDisabledAntiforgery = builder.UseDisabledAntiforgery;
        _useInMemoryDatabase = builder.UseInMemoryDatabase;
    }

    public static Builder Create()
    {
        return new Builder();
    }

    public TestQueryHandlerRegistry HandlerRegistry { get; }

    public ITestOutputHelper? OutputHelper { get; set; }

    public StubFallbackProviderResolver FallbackProviderResolver { get; }

    public string StartKestrel()
    {
        if (_baseUrl is not null)
            return _baseUrl;

        UseKestrel(port: 0);
        StartServer();

        var server = Services.GetRequiredService<IServer>();
        var addresses = server.Features.Get<IServerAddressesFeature>()!.Addresses;

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
        if (_useInMemoryDatabase)
        {
            services.ReplaceWithInMemory<IApplicationDbContext, ApplicationDbContext>();
        }

        services.ReplaceWithSingleton<IFallbackProviderResolver>(_ => FallbackProviderResolver);

        if (_useDisabledAntiforgery)
        {
            services.ReplaceWithSingleton<IAntiforgery, DisabledAntiforgery>();
        }

        if (_useSessionAuthentication)
        {
            services
                .AddAuthentication(SessionAuthenticationHandler.SchemeName)
                .AddScheme<AuthenticationSchemeOptions, SessionAuthenticationHandler>(
                    SessionAuthenticationHandler.SchemeName,
                    _ => { }
                );
        }

        if (_useFakePolicyEvaluator)
        {
            services.AddTransient<IPolicyEvaluator, FakePolicyEvaluator>();
        }

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
