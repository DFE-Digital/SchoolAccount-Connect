using SchoolAccount.Kernel;
using SchoolAccount.Web.Manage.Authentication;
using SchoolAccount.Web.Manage.Models;
using System.Reflection;
using Azure.Identity;
using FluentValidation;
using FluentValidation.AspNetCore;
using GovUk.Frontend.AspNetCore;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.FeatureFilters;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

namespace SchoolAccount.Web.Manage;

internal static class DependencyInjection
{
    internal static void AddPresentation(this IServiceCollection services, ConfigurationManager configurationManager)
    {
        services.AddFluentValidation();
        services.AddGovUkFrontend(options =>
        {
            options.Rebrand = true;
        });
        
        services.AddAntiforgery();
        services.AddHttpContextAccessor();
        services.AddScoped<IUserContext, UserContext>();
        
        services.Configure<TopHeaderNavigationOptions>(configurationManager.GetSection("TopHeaderNavigation"));
        
        services
            .AddControllersWithViews()
            .AddMicrosoftIdentityUI();
        
        services.AddAzureAuthentication(configurationManager);
        services.AddFeatureFlagSupport(configurationManager);
        
    }

    internal static void ConfigureAreas(this WebApplication app)
    {
        app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
    }

    internal static void ExceptionHandlers(this WebApplication app)
    {
        app.UseStatusCodePagesWithReExecute("/error/{0}");
        app.UseExceptionHandler("/error/500");
    }
    
    internal static void StripHeaders(this WebApplication app)
    {
        //Strips X-Powered-By header for security reasons
        app.Use(async (context, next) =>
        {
            context.Response.Headers.Remove("X-Powered-By");
            await next();
        });
    }
    
    private static void AddFluentValidation(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation(config =>
        {
            config.DisableDataAnnotationsValidation = true;
        });

        services.AddValidatorsFromAssemblies(
            [Assembly.GetExecutingAssembly(), typeof(Application.DependencyInjection).Assembly],
            includeInternalTypes: true
        );
    }

    private static void AddFeatureFlagSupport(this IServiceCollection services, ConfigurationManager configurationBuilder)
    {
        var appConfigEndpoint = configurationBuilder.GetValue<string>("AppConfigEndpoint");
        var managedIdentityClientId = configurationBuilder.GetValue<string>("MANAGED_IDENTITY_CLIENT_ID");
        var tenantId = configurationBuilder.GetValue<string>("TenantId");
        
        if (string.IsNullOrEmpty(appConfigEndpoint))
        {
            throw new ArgumentException("AppConfigEndpoint is required.");
        }

        var credential = new DefaultAzureCredential(new DefaultAzureCredentialOptions
        {
            TenantId = tenantId,
            ManagedIdentityClientId = managedIdentityClientId
        });

        configurationBuilder.AddAzureAppConfiguration(options =>
        {
            options.Connect(new Uri(appConfigEndpoint), credential)
                .Select(KeyFilter.Any)
                .ConfigureRefresh(refresh =>
                    refresh.RegisterAll()
                        .SetRefreshInterval(TimeSpan.FromSeconds(30)))
                .UseFeatureFlags();
        });

        services.AddAzureAppConfiguration();
        
        services.AddFeatureManagement()
            .AddFeatureFilter<TimeWindowFilter>()
            .AddFeatureFilter<PercentageFilter>();
    }
    
    private static void AddAzureAuthentication(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddAuthorization();
        
        services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApp(configuration.GetSection("AzureAd"));
    }
}
