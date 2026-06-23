using System.Diagnostics.CodeAnalysis;
using AngleSharp.Dom;
using Microsoft.AspNetCore.Mvc.Testing;

namespace SchoolAccount.Tests.Common.Fixtures;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.AllConstructors)]
public class KestrelServerFixture : WebApplicationFixture
{
    public string BaseUrl => Factory.StartKestrel();

    protected override HttpClient CreateClient(WebApplicationFactoryClientOptions? options = null)
    {
        _ = BaseUrl;
        return base.CreateClient(options);
    }

    public override Task<IDocument> RequestPageAsync(string uri, WebApplicationFactoryClientOptions? options = null)
    {
        _ = BaseUrl;
        return base.RequestPageAsync(uri, options);
    }
}
