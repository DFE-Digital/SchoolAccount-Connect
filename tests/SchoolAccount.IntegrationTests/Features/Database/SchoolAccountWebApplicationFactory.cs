using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Infrastructure;
using SchoolAccount.IntegrationTests.Fakes;

namespace SchoolAccount.IntegrationTests.Features.Database;

public class SchoolAccountWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup>
    where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("IntegrationTest");

        builder.ConfigureTestServices(services =>
        {
            services.AddTransient<IPolicyEvaluator, FakePolicyEvaluator>();
            services.AddTransient<IApplicationDbContext, ApplicationDbContext>();
            services.AddTransient<DbContextOptions<ApplicationDbContext>>();
        });
    }

    internal async Task ResetDatabaseAsync()
    {
        var context = GetDbContext();
        await context.Database.EnsureDeletedAsync();
        await context.SaveChangesAsync();
    }

    internal ApplicationDbContext GetDbContext()
    {
#pragma warning disable CA2000
        var scope = Services.CreateScope();
#pragma warning restore CA2000
        return scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    }
}
