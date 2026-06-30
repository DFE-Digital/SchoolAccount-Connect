using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc.Testing;
using SchoolAccount.Tests.Common.Builders;
using static SchoolAccount.Tests.Common.Factories.SchoolAccountWebApplicationFactory;

namespace SchoolAccount.Tests.Common.Fixtures;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.AllConstructors)]
public class KestrelServerFixture : WebApplicationFixture
{
    public KestrelServerFixture()
        : base() { }

    protected KestrelServerFixture(
        Func<Builder, Builder>? configureAnonymous,
        Func<Builder, Builder>? configureAuthenticated
    )
        : base(configureAnonymous, configureAuthenticated) { }

    public string GetAnonymousBaseUrl() => AnonymousFactory.StartKestrel();

    public string GetAuthenticatedBaseUrl() => AuthenticatedFactory.StartKestrel();

    public override HttpClient CreateAnonymousClient(Action<WebApplicationFactoryClientOptions>? configure = null)
    {
        _ = AnonymousFactory.StartKestrel();
        return base.CreateAnonymousClient(configure);
    }

    public override HttpClient CreateAuthenticatedClient(
        string? userId = null,
        OrganisationClaimBuilder? organisation = null,
        Action<WebApplicationFactoryClientOptions>? configure = null
    )
    {
        _ = AuthenticatedFactory.StartKestrel();
        return base.CreateAuthenticatedClient(userId, organisation, configure);
    }
}
