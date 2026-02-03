using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using SchoolAccount.Infrastructure;

namespace SchoolAccount.IntegrationTests.Factory;

internal class SchoolAccountWebApplicationFactory<TStartup> 
    : WebApplicationFactory<TStartup> where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test");

        builder.ConfigureServices(services =>
        {

        });
    }
}