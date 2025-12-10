using Azure.Identity;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using SchoolAccount.Kernel;
using SchoolAccount.Web.Manage.Authentication;
using System.Reflection;

namespace SchoolAccount.Web.Manage;

internal static class DependencyInjection
{
    internal static void AddPresentation(this IServiceCollection services, IConfigurationBuilder configurationBuilder)
    {
        services.AddFluentValidation();
        services.AddScoped<IUserContext, UserContext>();
        services.AddHttpContextAccessor();
        services.AddFeatureFlagSupport(configurationBuilder);
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
}
