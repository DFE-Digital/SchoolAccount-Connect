using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.Extensions.DependencyInjection;
using SchoolAccount.Infrastructure;
using SchoolAccount.Tests.Common;
using SchoolAccount.Tests.Common.Fakes;

namespace SchoolAccount.IntegrationTests.Features.Database;

[SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope")]
public class SchoolAccountWebApplicationFactory<TStartup> : SchoolAccountBaseWebApplicationFactory<TStartup>
    where TStartup : class
{
    protected override void ConfigureTestServices(IServiceCollection services)
    {
        services.AddTransient<IPolicyEvaluator, FakePolicyEvaluator>();
        base.ConfigureTestServices(services);
    }

    internal async Task ResetDatabaseAsync()
    {
        var context = GetDbContext();
        await context.Database.EnsureDeletedAsync();
        await context.SaveChangesAsync();
    }

    internal ApplicationDbContext GetDbContext()
    {
        var scope = Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    }
}
