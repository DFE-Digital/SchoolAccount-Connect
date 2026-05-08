using System.ComponentModel.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SchoolAccount.Integration.AcademiesApi.Configuration;
using SchoolAccount.Integration.AcademiesApi.Services;

namespace SchoolAccount.Integration.AcademiesApi;

public static class DependencyInjection
{
    public static IServiceCollection AddAcademiesApi(this IServiceCollection services,
        IConfigurationManager configuration, AcademiesApiOptions? options = null)
    {
        options ??= new AcademiesApiOptions();
        var section = configuration.GetSection(options.ConfigurationSectionName);

        if (!section.Exists())
        {
            return services;
        }
        
        services.AddOptions<AcademiesApiConfig>()
            .Bind(section)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        Action<IServiceProvider, HttpClient> setUpHttpClient = (serviceProvider, client) =>
        {
            var config = serviceProvider.GetRequiredService<IOptions<AcademiesApiConfig>>().Value;
            client.BaseAddress = new Uri(config.PublicUrl);
            client.DefaultRequestHeaders.Add("ApiKey", config.ApiKey);
        };
        
        services.AddHttpClient<IOrganisationApiService, OrganisationApiService>(setUpHttpClient);
        services.AddHttpClient<ITrustApiService, TrustApiService>(setUpHttpClient);
        
        return services;
    }
}