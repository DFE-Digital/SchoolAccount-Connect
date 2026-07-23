using Microsoft.AspNetCore.Mvc.Testing;
using SchoolAccount.Tests.Common.Builders;
using SchoolAccount.Tests.Common.Extensions;
using SchoolAccount.Tests.Common.Factories;
using SchoolAccount.Tests.Common.Fakes;
using Xunit;
using static SchoolAccount.Tests.Common.Factories.SchoolAccountWebApplicationFactory;

namespace SchoolAccount.Tests.Common.Fixtures;

public abstract class WebApplicationFixture : IAsyncLifetime
{
    protected WebApplicationFixture(
        Func<Builder, Builder>? configureAnonymous = null,
        Func<Builder, Builder>? configureAuthenticated = null
    )
    {
        AnonymousFactory = BuildFactory(
            configureAnonymous
                ?? (
                    b =>
                        b.WithTestDoubles(HandlerRegistry, FallbackProviderResolver)
                            .WithoutAuthentication()
                            .WithDisabledAntiforgery()
                )
        );

        AuthenticatedFactory = BuildFactory(
            configureAuthenticated
                ?? (
                    b =>
                        b.WithTestDoubles(HandlerRegistry, FallbackProviderResolver)
                            .WithAuthentication()
                            .WithDisabledAntiforgery()
                )
        );
    }

    public TestQueryHandlerRegistry HandlerRegistry => field ??= new TestQueryHandlerRegistry();

    public StubFallbackProviderResolver FallbackProviderResolver => field ??= new StubFallbackProviderResolver();

    protected SchoolAccountWebApplicationFactory AnonymousFactory { get; }

    protected SchoolAccountWebApplicationFactory AuthenticatedFactory { get; }

    public void SetOutputHelper(ITestOutputHelper? outputHelper)
    {
        AnonymousFactory.OutputHelper = outputHelper;
        AuthenticatedFactory.OutputHelper = outputHelper;
    }

    public virtual HttpClient CreateAnonymousClient(Action<WebApplicationFactoryClientOptions>? configure = null)
    {
        var options = BuildOptions(AnonymousFactory.ClientOptions, configure);

        return AnonymousFactory.CreateClient(options);
    }

    public virtual HttpClient CreateAuthenticatedClient(
        string? userId = null,
        OrganisationClaimBuilder? organisation = null,
        Action<WebApplicationFactoryClientOptions>? configure = null
    )
    {
        var options = BuildOptions(AuthenticatedFactory.ClientOptions, configure);

        return AuthenticatedFactory.CreateClient(options).WithAuthentication(userId, organisation);
    }

    public ValueTask InitializeAsync() => ValueTask.CompletedTask;

    public async ValueTask DisposeAsync()
    {
        await AuthenticatedFactory.DisposeAsync();
        await AnonymousFactory.DisposeAsync();
        GC.SuppressFinalize(this);
    }

    private static WebApplicationFactoryClientOptions BuildOptions(
        WebApplicationFactoryClientOptions? defaults = null,
        Action<WebApplicationFactoryClientOptions>? configure = null
    )
    {
        var options = defaults ?? new WebApplicationFactoryClientOptions();
        configure?.Invoke(options);
        return options;
    }

    private static SchoolAccountWebApplicationFactory BuildFactory(Func<Builder, Builder> configure) =>
        configure(Create()).Build();
}
