using AngleSharp.Dom;
using Microsoft.AspNetCore.Mvc.Testing;
using SchoolAccount.IntegrationTests.Testing;
using SchoolAccount.Tests.Common.Extensions;
using SchoolAccount.Tests.Common.Fakes;
using Xunit;

namespace SchoolAccount.IntegrationTests.Fixtures;

public abstract class WebApplicationFixture : IAsyncLifetime
{
    protected SchoolAccountWebApplicationFactory Factory { get; } = new();

    public TestQueryHandlerRegistry HandlerRegistry => Factory.HandlerRegistry;
    public StubFallbackProviderResolver FallbackProviderResolver => Factory.FallbackProviderResolver;

    public ITestOutputHelper? OutputHelper
    {
        get => Factory.OutputHelper;
        set => Factory.OutputHelper = value;
    }

    protected virtual HttpClient CreateClient(WebApplicationFactoryClientOptions? options = null)
    {
        return Factory.CreateClient(options ?? Factory.ClientOptions);
    }

    public virtual Task<IDocument> RequestPageAsync(string uri, WebApplicationFactoryClientOptions? options = null)
    {
        return Factory.RequestPageAsync(uri, options);
    }

    public ValueTask InitializeAsync() => ValueTask.CompletedTask;

    public async ValueTask DisposeAsync()
    {
        await Factory.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}
