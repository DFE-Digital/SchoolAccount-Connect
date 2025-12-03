using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace SchoolAccount.Web.Manage;

internal static class DependencyInjection
{
    internal static void AddPresentation(this IServiceCollection services)
    {
        services.AddFluentValidation();
    }
    
    private static void AddFluentValidation(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation(config =>
        {
            config.DisableDataAnnotationsValidation = true;
        });

        services.AddValidatorsFromAssemblies(
        [
            Assembly.GetExecutingAssembly(),
            typeof(Application.DependencyInjection).Assembly
        ], includeInternalTypes: true);
    }
}


