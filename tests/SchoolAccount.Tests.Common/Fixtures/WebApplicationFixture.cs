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
    private readonly SchoolAccountWebApplicationFactory _authenticatedFactory;

    protected WebApplicationFixture()
    {
        Factory = new SchoolAccountWebApplicationFactory(_handlerRegistry, _fallbackProviderResolver);
        _authenticatedFactory = new SchoolAccountWebApplicationFactory(
            _handlerRegistry,
            _fallbackProviderResolver,
            useSessionAuthentication: true,
            useFakePolicyEvaluator: false
        );
    }

    protected SchoolAccountWebApplicationFactory Factory { get; }

    public TestQueryHandlerRegistry HandlerRegistry => _handlerRegistry;
    public StubFallbackProviderResolver FallbackProviderResolver => _fallbackProviderResolver;

    public ITestOutputHelper? OutputHelper
    {
        get => Factory.OutputHelper;
        set
        {
            Factory.OutputHelper = value;
            _authenticatedFactory.OutputHelper = value;
        }
    }

    protected virtual HttpClient CreateClient(WebApplicationFactoryClientOptions? options = null)
    {
        return Factory.CreateClient(options ?? Factory.ClientOptions);
    }

    public HttpClient CreateAuthenticatedClient(string? userId = null, OrganisationClaimBuilder? organisation = null)
    {
        SessionAuthenticationHandler.CurrentUserId = userId ?? SessionAuthenticationHandler.DefaultUserId;
        SessionAuthenticationHandler.OrganisationClaim = organisation;

        return _authenticatedFactory.CreateClient(
            new WebApplicationFactoryClientOptions { HandleCookies = true, AllowAutoRedirect = true }
        );
    }

    public virtual Task<IDocument> RequestPageAsync(string uri, WebApplicationFactoryClientOptions? options = null)
    {
        return Factory.RequestPageAsync(uri, options);
    }

    public ValueTask InitializeAsync() => ValueTask.CompletedTask;

    public async ValueTask DisposeAsync()
    {
        await _authenticatedFactory.DisposeAsync();
        await Factory.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}
