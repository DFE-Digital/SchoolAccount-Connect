using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using SchoolAccount.Kernel;
using SchoolAccount.Web.Manage.Authentication;

namespace SchoolAccount.Web.Manage;

internal static class DependencyInjection
{
    internal static void AddPresentation(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddFluentValidation();
        services.AddScoped<IUserContext, UserContext>();
        services.AddHttpContextAccessor();
        
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
    
    private static void AddAzureAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthorization();
        
        services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApp(configuration.GetSection("AzureAd"));
    }

    internal static void UseStripHeaders(this WebApplication app)
    {
        app.Use(async (context, next) =>
        {
            context.Response.Headers.Remove("X-Powered-By");
            await next();
        });
    }
}
