using AngleSharp.Dom;
using Microsoft.AspNetCore.Mvc.Testing;
using SchoolAccount.Tests.Common.Builders;
using SchoolAccount.Tests.Common.Extensions;
using SchoolAccount.Tests.Common.Factories;
using SchoolAccount.Tests.Common.Fakes;
using Xunit;

namespace SchoolAccount.Tests.Common.Fixtures;

public abstract class WebApplicationFixture : IAsyncLifetime
{
    private readonly TestQueryHandlerRegistry _handlerRegistry = new();
    private readonly StubFallbackProviderResolver _fallbackProviderResolver = new();

    protected WebApplicationFixture()
    {
        UnauthenticatedFactory = new SchoolAccountWebApplicationFactory(_handlerRegistry, _fallbackProviderResolver);
        AuthenticatedFactory = new SchoolAccountWebApplicationFactory(
            _handlerRegistry,
            _fallbackProviderResolver,
            useSessionAuthentication: true,
            useFakePolicyEvaluator: false
        );
    }

    private SchoolAccountWebApplicationFactory UnauthenticatedFactory { get; }

    private SchoolAccountWebApplicationFactory AuthenticatedFactory { get; }

    protected abstract WebApplicationAccessMode DefaultAccessMode { get; }

    public TestQueryHandlerRegistry HandlerRegistry => _handlerRegistry;
    public StubFallbackProviderResolver FallbackProviderResolver => _fallbackProviderResolver;

    public ITestOutputHelper? OutputHelper
    {
        get => UnauthenticatedFactory.OutputHelper;
        set
        {
            UnauthenticatedFactory.OutputHelper = value;
            AuthenticatedFactory.OutputHelper = value;
        }
    }

    protected virtual HttpClient CreateClient(WebApplicationFactoryClientOptions? options = null)
    {
        return CreateClient(DefaultAccessMode, options);
    }

    protected virtual HttpClient CreateClient(
        WebApplicationAccessMode accessMode,
        WebApplicationFactoryClientOptions? options = null
    )
    {
        var factory = GetFactory(accessMode);
        return factory.CreateClient(options ?? factory.ClientOptions);
    }

    public HttpClient CreateAuthenticatedClient(string? userId = null, OrganisationClaimBuilder? organisation = null)
    {
        SessionAuthenticationHandler.CurrentUserId = userId ?? SessionAuthenticationHandler.DefaultUserId;
        SessionAuthenticationHandler.OrganisationClaim = organisation;

        return CreateClient(
            WebApplicationAccessMode.Authenticated,
            new WebApplicationFactoryClientOptions { HandleCookies = true, AllowAutoRedirect = true }
        );
    }

    public virtual Task<IDocument> RequestPageAsync(string uri, WebApplicationFactoryClientOptions? options = null)
    {
        return RequestPageAsync(uri, DefaultAccessMode, options);
    }

    public virtual Task<IDocument> RequestPageAsync(
        string uri,
        WebApplicationAccessMode accessMode,
        WebApplicationFactoryClientOptions? options = null
    )
    {
        return GetFactory(accessMode).RequestPageAsync(uri, options);
    }

    public ValueTask InitializeAsync() => ValueTask.CompletedTask;

    public async ValueTask DisposeAsync()
    {
        await AuthenticatedFactory.DisposeAsync();
        await UnauthenticatedFactory.DisposeAsync();
        GC.SuppressFinalize(this);
    }

    protected SchoolAccountWebApplicationFactory GetFactory(WebApplicationAccessMode accessMode)
    {
        return accessMode switch
        {
            WebApplicationAccessMode.Unauthenticated => UnauthenticatedFactory,
            WebApplicationAccessMode.Authenticated => AuthenticatedFactory,
            _ => throw new ArgumentOutOfRangeException(nameof(accessMode), accessMode, null),
        };
    }
}
