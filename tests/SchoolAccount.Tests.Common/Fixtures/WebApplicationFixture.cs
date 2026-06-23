using System.Text.Json;
using AngleSharp.Dom;
using Microsoft.AspNetCore.Mvc.Testing;
using SchoolAccount.Tests.Common.Builders;
using SchoolAccount.Tests.Common.Extensions;
using SchoolAccount.Tests.Common.Factories;
using SchoolAccount.Tests.Common.Fakes;
using Xunit;
using static SchoolAccount.Tests.Common.Fakes.SessionAuthenticationHandler;
using static SchoolAccount.Tests.Common.Fixtures.WebApplicationAccessMode;

namespace SchoolAccount.Tests.Common.Fixtures;

public abstract class WebApplicationFixture : IAsyncLifetime
{
    protected WebApplicationFixture()
    {
        UnauthenticatedFactory = BuildFactory(b => b.WithFakePolicyEvaluator());
        AuthenticatedFactory = BuildFactory(b => b.WithSessionAuthentication());
    }

    public TestQueryHandlerRegistry HandlerRegistry { get; } = new();

    public StubFallbackProviderResolver FallbackProviderResolver { get; } = new();

    public ITestOutputHelper? OutputHelper
    {
        get => UnauthenticatedFactory.OutputHelper;
        set
        {
            UnauthenticatedFactory.OutputHelper = value;
            AuthenticatedFactory.OutputHelper = value;
        }
    }

    protected SchoolAccountWebApplicationFactory UnauthenticatedFactory { get; }

    protected SchoolAccountWebApplicationFactory AuthenticatedFactory { get; }

    protected abstract WebApplicationAccessMode DefaultAccessMode { get; }

    public HttpClient CreateAuthenticatedClient(string? userId = null, OrganisationClaimBuilder? organisation = null)
    {
        var options = new WebApplicationFactoryClientOptions { HandleCookies = true, AllowAutoRedirect = true };
        var client = CreateClient(Authenticated, options);

        AddAuthenticationHeaders(client, userId, organisation);

        return client;
    }

    public virtual Task<IDocument> RequestPageAsync(string uri, WebApplicationFactoryClientOptions? options = null) =>
        RequestPageAsync(uri, DefaultAccessMode, options);

    public virtual Task<IDocument> RequestPageAsync(
        string uri,
        WebApplicationAccessMode accessMode,
        WebApplicationFactoryClientOptions? options = null
    ) => GetFactory(accessMode).RequestPageAsync(uri, options);

    public ValueTask InitializeAsync() => ValueTask.CompletedTask;

    public async ValueTask DisposeAsync()
    {
        await AuthenticatedFactory.DisposeAsync();
        await UnauthenticatedFactory.DisposeAsync();
        GC.SuppressFinalize(this);
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

    protected SchoolAccountWebApplicationFactory GetFactory(WebApplicationAccessMode accessMode)
    {
        return accessMode switch
        {
            Unauthenticated => UnauthenticatedFactory,
            Authenticated => AuthenticatedFactory,
            _ => throw new ArgumentOutOfRangeException(nameof(accessMode), accessMode, null),
        };
    }

    private static void AddAuthenticationHeaders(
        HttpClient client,
        string? userId,
        OrganisationClaimBuilder? organisation
    )
    {
        client.DefaultRequestHeaders.Add(UserIdHeader, userId ?? DefaultUserId);
        client.DefaultRequestHeaders.Add(OrganisationHeader, SerialiseOrganisationClaim(organisation));
    }

    private static string SerialiseOrganisationClaim(OrganisationClaimBuilder? organisation)
    {
        var organisationClaim = (organisation ?? OrganisationClaimBuilder.Default).Build();

        return JsonSerializer.Serialize(organisationClaim, JsonSerializerOptions.Web);
    }

    private SchoolAccountWebApplicationFactory BuildFactory(
        Func<SchoolAccountWebApplicationFactory.Builder, SchoolAccountWebApplicationFactory.Builder>? configure = null
    )
    {
        var builder = SchoolAccountWebApplicationFactory
            .Create()
            .WithTestDoubles(HandlerRegistry, FallbackProviderResolver);

        if (configure is not null)
            builder = configure(builder);

        return builder.Build();
    }
}
