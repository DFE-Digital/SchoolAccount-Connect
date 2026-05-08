using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SchoolAccount.Integration.DfESignIn.Configuration;
using SchoolAccount.Integration.DfESignIn.Services;

namespace SchoolAccount.Integration.DfESignIn;

public static class DependencyInjection
{
    public static IServiceCollection AddDsiApi(this IServiceCollection services,
        IConfigurationManager configuration, DsiApiOptions? options = null)
    {
        options ??= new DsiApiOptions();
        var section = configuration.GetSection(options.ConfigurationSectionName);

        if (!section.Exists())
        {
            return services;
        }
        
        services.AddOptions<DsiApiConfig>()
            .Bind(section)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        
        services.AddHttpClient<IDsiApiService, DsiApiService>((serviceProvider, client) =>
        {
            var config = serviceProvider.GetRequiredService<IOptions<DsiApiConfig>>().Value;
            client.BaseAddress = new Uri(config.PublicUrl);
        });
        
        return services;
    }
}