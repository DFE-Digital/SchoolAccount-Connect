using Azure.Identity;
using FluentValidation;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using SchoolAccount.Web.Connect.Settings;
using SchoolAccount.Web.Connect.Validation.Validators;

namespace SchoolAccount.Web.Connect.Extensions;

public static class AzureAppConfigurationExtensions
{
    public static void AddAzureAppConfigurationIfEnabled(this IConfigurationBuilder configBuilder, ILogger logger)
    {
        logger.LogInformation("Loading Azure App Configuration...");

        var settings = ValidateAndGetConfiguration(configBuilder);

        if (!settings.Enabled)
        {
            logger.LogWarning("Azure App Configuration is disabled. Skipping configuration setup.");
            return;
        }

        configBuilder.AddAzureAppConfiguration(options =>
        {
            if (settings.IsEmulated)
            {
                ConnectToEmulatorEndpoint(settings, options);
            }
            else
            {
                ConnectToAzureEndpoint(settings, options);
            }

            options.UseFeatureFlags();
        });

        logger.LogInformation("Azure App Configuration loaded successfully");
    }

    public static IApplicationBuilder UseAzureAppConfigurationIfEnabled(
        this IApplicationBuilder builder,
        IConfigurationBuilder configBuilder
    )
    {
        var appConfigurationSettings = ValidateAndGetConfiguration(configBuilder);

        if (appConfigurationSettings.Enabled)
        {
            builder.UseAzureAppConfiguration();
        }

        return builder;
    }

    public static IServiceCollection AddAzureAppConfigurationIfEnabled(
        this IServiceCollection services,
        IConfigurationBuilder configBuilder
    )
    {
        var appConfigurationSettings = ValidateAndGetConfiguration(configBuilder);

        if (appConfigurationSettings.Enabled)
        {
            services.AddAzureAppConfiguration();
        }

        return services;
    }

    private static AzureAppConfigurationSettings ValidateAndGetConfiguration(IConfigurationBuilder configBuilder)
    {
        var validator = new AzureAppConfigurationSettingsValidator();
        var configuration = configBuilder.Build();

        var settings = configuration
            .GetRequiredSection(AzureAppConfigurationSettings.SectionName)
            .Get<AzureAppConfigurationSettings>();

        if (settings is null)
        {
            throw new NullReferenceException($"Could not bind {nameof(AzureAppConfigurationSettings)}.");
        }

        var result = validator.Validate(settings);

        if (result.IsValid)
            return settings;

        var errors = string.Join("; ", result.Errors.Select(e => e.ErrorMessage));
        throw new ValidationException($"Configuration validation failed: {errors}");
    }

    private static void ConfigureRefresh(AzureAppConfigurationOptions options, TimeSpan cacheExpiration)
    {
        options
            .Select(KeyFilter.Any)
            .ConfigureRefresh(refresh => refresh.RegisterAll().SetRefreshInterval(cacheExpiration));
    }

    private static void ConnectToEmulatorEndpoint(
        AzureAppConfigurationSettings settings,
        AzureAppConfigurationOptions options
    )
    {
        ConfigureRefresh(options, TimeSpan.FromSeconds(5));

        options.ReplicaDiscoveryEnabled = false;
        options.Connect(settings.Endpoint);
    }

    private static void ConnectToAzureEndpoint(
        AzureAppConfigurationSettings settings,
        AzureAppConfigurationOptions options
    )
    {
        var credential = new DefaultAzureCredential(new DefaultAzureCredentialOptions());
        var endpoint = new Uri(settings.Endpoint);

        ConfigureRefresh(options, TimeSpan.FromSeconds(30));

        options.Connect(endpoint, credential).ConfigureKeyVault(kv => kv.SetCredential(credential));
    }
}
