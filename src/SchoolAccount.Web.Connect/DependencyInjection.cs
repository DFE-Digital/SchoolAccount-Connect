using FluentValidation;
using FluentValidation.AspNetCore;
using GovUk.Frontend.AspNetCore;
using Microsoft.Identity.Web.UI;
using SchoolAccount.Kernel;
using SchoolAccount.Web.Connect.Authentication;
using SchoolAccount.Web.Connect.Models;
using SchoolAccount.Web.Connect.SignIn;
using System.Reflection;
using Microsoft.FeatureManagement;
using SchoolAccount.Integration.DfESignIn.Interfaces;

namespace SchoolAccount.Web.Connect;

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
        services.AddScoped<IOrganisationContext, OrganisationContext>();
        services.AddFeatureToggle();
        
        services.Configure<TopHeaderNavigationOptions>(configurationManager.GetSection("TopHeaderNavigation"));
        
        services
            .AddControllersWithViews()
            .AddMicrosoftIdentityUI();
        
        services.AddDfeSignInAuthentication(configurationManager);
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

    private static void AddFeatureToggle(this IServiceCollection services)
    {
        services.AddAzureAppConfiguration();
        services.AddFeatureManagement();
    }
}
