using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using GovUk.Frontend.AspNetCore;
using SchoolAccount.Kernel;
using SchoolAccount.Web.Manage.Authentication;
using SchoolAccount.Web.Manage.Models;

namespace SchoolAccount.Web.Manage;

internal static class DependencyInjection
{
    internal static void AddPresentation(this IServiceCollection services)
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
    }
    
    internal static void Configure(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<TopHeaderNavigationOptions>(builder.Configuration.GetSection("TopHeaderNavigation"));
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
    }
}
