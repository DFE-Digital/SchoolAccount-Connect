using Azure.Identity;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;

namespace SchoolAccount.Web.Manage.Infrastructure
{
    public static class AzureAppConfigurationExtension
    {
        public static void AddAzureConfigurations(this IConfigurationBuilder configurationBuilder, string appConfigEndpoint,
            string? managedIdentityId, string? tenantId)
        {
            ConfigureAzureAppConfiguration(configurationBuilder, appConfigEndpoint, managedIdentityId, tenantId);
        }

        private static void ConfigureAzureAppConfiguration(IConfigurationBuilder configurationBuilder,
            string appConfigEndpoint, string? managedIdentityId, string? tenantId)
        {
            var credential = new DefaultAzureCredential(new DefaultAzureCredentialOptions
            {
                TenantId = tenantId,
                ManagedIdentityClientId = managedIdentityId
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
}
