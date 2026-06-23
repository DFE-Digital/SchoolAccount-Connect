using System.Diagnostics.CodeAnalysis;
using AngleSharp.Dom;
using Microsoft.AspNetCore.Mvc.Testing;
using static SchoolAccount.Tests.Common.Fixtures.WebApplicationAccessMode;

namespace SchoolAccount.Tests.Common.Fixtures;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.AllConstructors)]
public class KestrelServerFixture : WebApplicationFixture
{
    protected override WebApplicationAccessMode DefaultAccessMode => Unauthenticated;

    public string GetBaseUrl(WebApplicationAccessMode accessMode) => StartServer(accessMode);

    protected override HttpClient CreateClient(
        WebApplicationAccessMode accessMode,
        WebApplicationFactoryClientOptions? options = null
    )
    {
        _ = StartServer(accessMode);
        return base.CreateClient(accessMode, options);
    }

    public override Task<IDocument> RequestPageAsync(
        string uri,
        WebApplicationAccessMode accessMode,
        WebApplicationFactoryClientOptions? options = null
    )
    {
        _ = StartServer(accessMode);
        return base.RequestPageAsync(uri, accessMode, options);
    }

    private string StartServer(WebApplicationAccessMode accessMode)
    {
        return GetFactory(accessMode).StartKestrel();
    }
}
