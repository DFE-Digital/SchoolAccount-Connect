using System.Reflection;
using Azure.Monitor.OpenTelemetry.AspNetCore;
using Azure.Monitor.OpenTelemetry.Exporter;
using FluentValidation;
using FluentValidation.AspNetCore;
using GovUk.Frontend.AspNetCore;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.FeatureFilters;
using Microsoft.Identity.Web.UI;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using SchoolAccount.Application.Abstractions.Telemetry;
using SchoolAccount.Kernel;
using SchoolAccount.Web.Connect.Authentication;
using SchoolAccount.Web.Connect.Builders;
using SchoolAccount.Web.Connect.Builders.Interfaces;
using SchoolAccount.Web.Connect.Extensions;
using SchoolAccount.Web.Connect.Infrastructure;
using SchoolAccount.Web.Connect.Middleware;
using SchoolAccount.Web.Connect.Middleware.Gates;
using SchoolAccount.Web.Connect.Middleware.Interfaces;
using SchoolAccount.Web.Connect.Models;
using SchoolAccount.Web.Connect.SignIn;
using SchoolAccount.Web.Connect.Telemetry;

namespace SchoolAccount.Web.Connect;

internal static class DependencyInjection
{
    internal static void AddPresentation(
        this IServiceCollection services,
        IConfigurationManager configurationManager,
        IWebHostEnvironment environment,
        ILogger bootstrapLogger
    )
    {
        configurationManager.AddAzureAppConfigurationIfEnabled(bootstrapLogger);

        services.AddFluentValidation();
        services.AddGovUkFrontend(options =>
        {
            options.Rebrand = true;
        });

        services.AddAntiforgery();
        services.AddHttpContextAccessor();
        services.AddContexts();
        services.AddAzureAppConfigurationIfEnabled(configurationManager);
        services.AddFeatureToggle();
        services.AddApplicationTelemetry(configurationManager, environment, bootstrapLogger);
        services.AddRequestGates();

        services.Configure<TopHeaderNavigationOptions>(configurationManager.GetSection("TopHeaderNavigation"));
        services.AddScoped<IFeedbackTelemetryService, FeedbackTelemetryService>();

        services.AddScoped<IPaginationViewBuilder, PaginationViewBuilder>();
        services.AddScoped<IDashboardViewBuilder, DashboardViewBuilder>();
        services.AddScoped<ICalendarOfItemsViewBuilder, CalendarOfItemsViewBuilder>();
        services.AddScoped<ICalendarOfItemsRowViewBuilder, CalendarOfItemsRowViewBuilder>();

        services.AddControllersWithViews().AddMicrosoftIdentityUI();

        services.AddDfeSignInAuthentication(configurationManager);
        services.AddSession();
    }

    internal static void AddPresentation(
        this ILoggingBuilder logging,
        IConfigurationManager config,
        IWebHostEnvironment environment
    )
    {
        var appInsightsConnectionString = config["ApplicationInsights:ConnectionString"];

        if (environment.IsDevelopment())
        {
            logging.AddConsole();
        }

        logging.AddOpenTelemetry(options =>
        {
            options.IncludeFormattedMessage = true;
            options.IncludeScopes = true;
            options.ParseStateValues = true;

            options.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(environment.ApplicationName));

            if (!string.IsNullOrWhiteSpace(appInsightsConnectionString))
            {
                options.AddAzureMonitorLogExporter(exporterOptions =>
                {
                    exporterOptions.ConnectionString = appInsightsConnectionString;
                });
            }

            var seqEndpoint = config["Seq:Endpoint"];

            if (!string.IsNullOrWhiteSpace(seqEndpoint))
            {
                options.AddOtlpExporter(exporterOptions =>
                {
                    exporterOptions.Protocol = OtlpExportProtocol.HttpProtobuf;
                    exporterOptions.Endpoint = new Uri(seqEndpoint);
                });
            }
        });
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
        app.MapControllerRoute(name: "areas", pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

        app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
    }

    internal static void ExceptionHandlers(this WebApplication app)
    {
        app.UseStatusCodePagesWithReExecute("/error/{0}");
        app.UseExceptionHandler("/error/500");
    }

    internal static void StripHeaders(this WebApplication app)
    {
        app.Use(
            async (context, next) =>
            {
                context.Response.Headers.Remove("X-Powered-By");
                await next();
            }
        );
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

    private static void AddFeatureToggle(this IServiceCollection services)
    {
        services
            .AddFeatureManagement()
            .AddFeatureFilter<PercentageFilter>()
            .AddFeatureFilter<TimeWindowFilter>()
            .AddFeatureFilter<TargetingFilter>();

        services.AddSingleton<ITargetingContextAccessor, FeatureManagementContextAccessor>();
    }

    private static void AddApplicationTelemetry(
        this IServiceCollection services,
        IConfigurationManager configurationManager,
        IHostEnvironment environment,
        ILogger logger
    )
    {
        var appInsightsConnectionString = configurationManager["ApplicationInsights:ConnectionString"];

        if (string.IsNullOrWhiteSpace(appInsightsConnectionString))
        {
            logger.LogWarning("No app insights connection string found. Skipping app insights setup.");
            return;
        }

        services
            .AddOpenTelemetry()
            .UseAzureMonitor(options =>
            {
                options.ConnectionString = appInsightsConnectionString;
            })
            .ConfigureResource(resource => resource.AddService(environment.ApplicationName))
            .WithMetrics(metrics =>
            {
                metrics.AddMeter("SchoolAccount.Feedback");
            });
    }

    public static void AddMiddleware(this WebApplication app)
    {
        app.UseMiddleware<RequestGateMiddleware>();
    }
}
