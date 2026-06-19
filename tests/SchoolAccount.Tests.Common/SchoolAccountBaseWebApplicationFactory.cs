using MartinCostello.Logging.XUnit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Infrastructure;
using SchoolAccount.Tests.Common.Extensions;
using SchoolAccount.Tests.Common.Fakes;
using Xunit;

namespace SchoolAccount.Tests.Common;

public class SchoolAccountBaseWebApplicationFactory<TStartup>
    : WebApplicationFactory<TStartup>,
        ITestOutputHelperAccessor
    where TStartup : class
{
    public ITestOutputHelper? OutputHelper { get; set; }

    public StubFallbackProviderResolver FallbackProviderResolver { get; } = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("IntegrationTest");
        builder.ConfigureLogging(p => p.AddXUnit(this));
        builder.ConfigureTestServices(ConfigureTestServices);
    }

    protected virtual void ConfigureTestServices(IServiceCollection services)
    {
        services.ReplaceWithInMemory<IApplicationDbContext, ApplicationDbContext>();
        services.ReplaceWithSingleton<IFallbackProviderResolver>(x => FallbackProviderResolver);
    }
}
