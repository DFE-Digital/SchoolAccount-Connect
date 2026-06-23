using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc.Testing;
using SchoolAccount.Tests.Common.Factories;
using Xunit;

namespace SchoolAccount.AuthenticationTests;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.AllConstructors)]
public class RoutingTestFixture : IAsyncLifetime
{
    private readonly SchoolAccountWebApplicationFactory _factory;

    public RoutingTestFixture()
    {
        _factory = SchoolAccountWebApplicationFactory.Create().WithInMemoryDatabase().Build();
    }

    public HttpClient CreateClient(WebApplicationFactoryClientOptions? options = null) =>
        _factory.CreateClient(options ?? _factory.ClientOptions);

    public ValueTask InitializeAsync() => ValueTask.CompletedTask;

    public async ValueTask DisposeAsync()
    {
        await _factory.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}
