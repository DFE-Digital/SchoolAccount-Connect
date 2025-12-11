using SchoolAccount.Kernel;
using SchoolAccount.Web.Manage.Authentication;
using SchoolAccount.Web.Manage.Models;
using System.Reflection;

namespace SchoolAccount.Web.Manage;

internal static class DependencyInjection
{
    internal static void AddPresentation(this IServiceCollection services, IConfigurationBuilder configurationBuilder)
    {
        services.AddFluentValidation();
        services.AddGovUkFrontend(options =>
        {
            options.Rebrand = true;
        });
        services.AddAntiforgery();
        services.AddHttpContextAccessor();
        services.AddControllersWithViews();
        services.AddScoped<IUserContext, UserContext>();
        services.AddFeatureFlagSupport(configurationBuilder);
    }
    
    internal static void Configure(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<TopHeaderNavigationOptions>(builder.Configuration.GetSection("TopHeaderNavigation"));
        
        services
            .AddControllersWithViews()
            .AddMicrosoftIdentityUI();
        
        services.AddAzureAuthentication(configuration);
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
        app.Use(async (context, next) =>
        {
            context.Response.Headers.Remove("X-Powered-By");
            await next();
        });
    }

    private static void AddFeatureFlagSupport(this IServiceCollection services, IConfigurationBuilder configurationBuilder)
    {
        var appConfigEndpoint = services.BuildServiceProvider().GetRequiredService<IConfiguration>()["AppConfigEndpoint"];
        var managedIdentityClientId = services.BuildServiceProvider().GetRequiredService<IConfiguration>()["MANAGED_IDENTITY_CLIENT_ID"];
        var tenantId = services.BuildServiceProvider().GetRequiredService<IConfiguration>()["TenantId"];
        
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
    }
    
    private static void AddAzureAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthorization();
        
        services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApp(configuration.GetSection("AzureAd"));
    }
}
