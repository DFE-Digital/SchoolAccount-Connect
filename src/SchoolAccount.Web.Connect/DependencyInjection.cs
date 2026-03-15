using System.Reflection;
using Azure.Identity;
using Azure.Monitor.OpenTelemetry.AspNetCore;
using FluentValidation;
using FluentValidation.AspNetCore;
using GovUk.Frontend.AspNetCore;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.FeatureFilters;
using Microsoft.Identity.Web.UI;
using SchoolAccount.Application.Abstractions.Telemetry;
using SchoolAccount.Kernel;
using SchoolAccount.Web.Connect.Authentication;
using SchoolAccount.Web.Connect.Infrastructure;
using SchoolAccount.Web.Connect.Middleware;
using SchoolAccount.Web.Connect.Middleware.Gates;
using SchoolAccount.Web.Connect.Middleware.Interfaces;
using SchoolAccount.Web.Connect.Models;
using SchoolAccount.Web.Connect.SignIn;
using SchoolAccount.Web.Connect.Telemetry;
using ConfigurationManager = Microsoft.Extensions.Configuration.ConfigurationManager;

namespace SchoolAccount.Web.Connect;

internal static class DependencyInjection
{
    internal static void AddPresentation(this IServiceCollection services, ConfigurationManager configurationManager)
    {
        services.AddFluentValidation();
        services.AddGovUkFrontend(options => { options.Rebrand = true; });

        services.AddAntiforgery();
        services.AddHttpContextAccessor();
        services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
        services.AddContexts();
        services.AddFeatureToggle(configurationManager);
        services.AddApplicationTelemetry(configurationManager);
        services.AddRequestGates();

        services.Configure<TopHeaderNavigationOptions>(configurationManager.GetSection("TopHeaderNavigation"));
        services.AddScoped<IFeedbackTelemetryService, FeedbackTelemetryService>();

        services
            .AddControllersWithViews()
            .AddMicrosoftIdentityUI();

        services.AddDfeSignInAuthentication(configurationManager);
        services.AddSession();
    }

    private static void AddContexts(this IServiceCollection services)
    {
        services.AddScoped<IUserContext, UserContext>();
        services.AddScoped<IOrganisationContext, OrganisationContext>();
    }

    private static void AddRequestGates(this IServiceCollection services)
    {
        services.AddScoped<IRequestGate, MaintenanceRequestGate>();
        services.AddScoped<IRequestGate, MatAcceptanceRequestGate>();
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
        app.Use(async (context, next) =>
        {
            context.Response.Headers.Remove("X-Powered-By");
            await next();
        });
    }

    private static void AddFluentValidation(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation(config => { config.DisableDataAnnotationsValidation = true; });

        services.AddValidatorsFromAssemblies(
            [Assembly.GetExecutingAssembly(), typeof(Application.DependencyInjection).Assembly],
            includeInternalTypes: true
        );
    }

    private static void AddFeatureToggle(this IServiceCollection services, ConfigurationManager configurationManager)
    {
        AddAzureAppConfiguration(configurationManager);

        services.AddAzureAppConfiguration();

        services.AddFeatureManagement()
            .AddFeatureFilter<PercentageFilter>()
            .AddFeatureFilter<TimeWindowFilter>()
            .AddFeatureFilter<TargetingFilter>();

        services.AddSingleton<ITargetingContextAccessor, FeatureManagementContextAccessor>();
    }

    private static void AddApplicationTelemetry(this IServiceCollection services,
        ConfigurationManager configurationManager)
    {
        var appInsightsInstrumentationKey = configurationManager["AppInsightsInstrumentationKey"]
                                            ?? throw new InvalidOperationException(
                                                "Configuration value 'AppInsightsInstrumentationKey' is missing.");

        var appInsightsConnectionString = $"InstrumentationKey={appInsightsInstrumentationKey}";

        services.AddApplicationInsightsTelemetry(options =>
        {
            options.ConnectionString = appInsightsConnectionString;
        });

        services.AddOpenTelemetry()
            .UseAzureMonitor(options => { options.ConnectionString = appInsightsConnectionString; })
            .WithMetrics(metrics => { metrics.AddMeter("SchoolAccount.Feedback"); });
    }

    public static void AddMiddleware(this WebApplication app)
    {
        app.UseMiddleware<RequestGateMiddleware>();
    }

    private static void AddAzureAppConfiguration(ConfigurationManager configurationManager)
    {
        var appConfigUri = configurationManager["AppConfigUri"]
                           ?? throw new InvalidOperationException(
                               "Configuration value 'AppConfigUri' is missing.");

        var credential = new DefaultAzureCredential(new DefaultAzureCredentialOptions
        {
            TenantId = configurationManager["TenantId"] ?? string.Empty
        });

        configurationManager.AddAzureAppConfiguration(options =>
        {
            options.Connect(new Uri(appConfigUri), credential)
                .Select(KeyFilter.Any)
                .UseFeatureFlags()
                .ConfigureKeyVault(keyVault => { keyVault.SetCredential(credential); })
                .ConfigureRefresh(refresh =>
                {
                    refresh.RegisterAll()
                        .SetRefreshInterval(TimeSpan.FromSeconds(5));
                });
        });
    }
}