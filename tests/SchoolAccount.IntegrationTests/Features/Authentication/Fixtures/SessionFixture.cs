using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Infrastructure;
using SchoolAccount.IntegrationTests.Features.Authentication.Handlers;
using SchoolAccount.IntegrationTests.Features.Database;
using SchoolAccount.Tests.Common.Builders;

namespace SchoolAccount.IntegrationTests.Features.Authentication.Fixtures;

public class SessionFixture : SchoolAccountWebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("IntegrationTest");
        
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<IFallbackProviderResolver>();
            services.AddSingleton<IFallbackProviderResolver>(FallbackProviderResolver);
            
            services.AddTransient<IApplicationDbContext, ApplicationDbContext>();
            services.AddTransient<DbContextOptions<ApplicationDbContext>>();
            
            services.AddAuthentication(SessionAuthenticationHandler.SchemeName)
                .AddScheme<AuthenticationSchemeOptions, SessionAuthenticationHandler>(
                    SessionAuthenticationHandler.SchemeName, _ => { });
            
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            
            services.AddSingleton<IAntiforgery, DisabledAntiforgery>();
        });
    }
    
    public HttpClient CreateAuthenticatedClient(string? userId = null, OrganisationClaimBuilder? organisation = null)
    {
        if (!string.IsNullOrEmpty(userId))
        {
            SessionAuthenticationHandler.CurrentUserId = userId;
        }

        if (organisation is not null)
        {
            SessionAuthenticationHandler.OrganisationClaim = organisation;
        }

        return CreateClient(new WebApplicationFactoryClientOptions
        {
            HandleCookies = true,
            AllowAutoRedirect = true
        });
    }
}