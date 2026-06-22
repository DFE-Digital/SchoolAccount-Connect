using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using SchoolAccount.IntegrationTests.Testing;
using SchoolAccount.Tests.Common.Fakes;
using Xunit;

namespace SchoolAccount.IntegrationTests.Fixtures;

public class PlaywrightFixture : IAsyncLifetime
{
    public SchoolAccountWebApplicationFactory? Factory { get; private set; }
    public string BaseUrl { get; private set; } = null!;

    public TestQueryHandlerRegistry HandlerRegistry => Factory!.HandlerRegistry;
    public StubFallbackProviderResolver FallbackProviderResolver => Factory!.FallbackProviderResolver;

    public void SetOutputHelper(ITestOutputHelper outputHelper)
    {
        if (Factory is not null)
            Factory.OutputHelper = outputHelper;
    }

    public async ValueTask InitializeAsync()
    {
        var factory = new SchoolAccountWebApplicationFactory();

        try
        {
            Factory = factory;
            factory = null;

            Factory.UseKestrel(port: 0);
            Factory.StartServer();

            var addresses = Factory
                .Services.GetRequiredService<IServer>()
                .Features.Get<IServerAddressesFeature>()!
                .Addresses;

            BaseUrl = addresses.First();
        }
        catch
        {
            await (Factory?.DisposeAsync() ?? ValueTask.CompletedTask);
            Factory = null;
            throw;
        }
        finally
        {
            await (factory?.DisposeAsync() ?? ValueTask.CompletedTask);
        }
    }

    public async ValueTask DisposeAsync()
    {
        await (Factory?.DisposeAsync() ?? ValueTask.CompletedTask);
        GC.SuppressFinalize(this);
    }
}
