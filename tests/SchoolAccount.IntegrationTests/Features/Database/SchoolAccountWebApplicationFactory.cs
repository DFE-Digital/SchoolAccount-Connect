using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Infrastructure;
using SchoolAccount.IntegrationTests.Fakes;
using SchoolAccount.IntegrationTests.Features.Database.Resolvers;

namespace SchoolAccount.IntegrationTests.Features.Database;

public class SchoolAccountWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup>
    where TStartup : class
{
    public StubFallbackProviderResolver FallbackProviderResolver { get; } = new();
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("IntegrationTest");

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<IFallbackProviderResolver>();
            services.AddSingleton<IFallbackProviderResolver>(FallbackProviderResolver);
            
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
