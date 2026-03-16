using System.Data.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SchoolAccount.Infrastructure;

namespace SchoolAccount.IntegrationTests.Factory;

internal sealed class SchoolAccountWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup>
    where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test");

        builder.ConfigureAppConfiguration(
            (_, config) =>
            {
                var overrides = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase) { };

                config.AddInMemoryCollection(overrides!);
            }
        );

        builder.ConfigureTestServices(services =>
        {
            //todo move to helper
            var dbContextDescriptor = services.SingleOrDefault(d =>
                d.ServiceType == typeof(IDbContextOptionsConfiguration<ApplicationDbContext>)
            );

            services.Remove(dbContextDescriptor!);

            var dbConnectionDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbConnection));

            services.Remove(dbConnectionDescriptor!);

            services.AddSingleton<DbConnection>(container =>
            {
                var connection = new SqliteConnection("DataSource=:memory:");
                connection.Open();

                return connection;
            });

            //Todo: Seeding classes - will implement once actual application has data setup
            services.AddDbContext<ApplicationDbContext>(
                (container, options) =>
                {
                    var connection = container.GetRequiredService<DbConnection>();
                    options.UseSqlite(connection);
                }
            );

            // Register test authentication as the default scheme for integration tests
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = TestAuthHandler.SchemeName;
                    options.DefaultChallengeScheme = TestAuthHandler.SchemeName;
                    options.DefaultScheme = TestAuthHandler.SchemeName;
                })
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(TestAuthHandler.SchemeName, options => { });

            services.AddAuthorization();
        });
    }

    internal async Task ResetDatabaseAsync()
    {
        var context = GetDbContext();

        // Clear all data
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
