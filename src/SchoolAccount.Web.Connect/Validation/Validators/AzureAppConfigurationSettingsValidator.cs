using FluentValidation;
using SchoolAccount.Web.Connect.Settings;

namespace SchoolAccount.Web.Connect.Validation.Validators;

public class AzureAppConfigurationSettingsValidator : AbstractValidator<AzureAppConfigurationSettings>
{
    public AzureAppConfigurationSettingsValidator()
    {
        RuleFor(x => x.Enabled).NotNull().WithMessage("Enabled property must be set");

        When(
            x => x.Enabled,
            () =>
            {
                RuleFor(x => x.IsEmulated)
                    .NotNull()
                    .WithMessage("IsEmulated must be specified when Azure App Configuration is enabled");

                RuleFor(x => x.Endpoint)
                    .NotEmpty()
                    .WithMessage("Endpoint must not be empty when enabled")
                    .Custom(
                        (endpoint, context) =>
                        {
                            if (!IsValidUri(endpoint))
                            {
                                context.AddFailure("Endpoint must be a valid URI");
                            }
                        }
                    );
            }
        );
    }

    private static bool IsValidUri(string endpoint)
    {
        var cleanEndpoint = ExtractUri(endpoint);
        return Uri.TryCreate(cleanEndpoint, UriKind.Absolute, out var uri)
            && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
    }

    private static string ExtractUri(string endpoint)
    {
        // Extract URI from connection string format
        // Example: "Endpoint=http://app-config-emulator:8483;Id=emulator-test-id;Secret=abcdefghijklmnopqrstuvwxyz1234567890"
        // Result: "http://app-config-emulator:8483"

        if (endpoint.StartsWith("Endpoint=", StringComparison.OrdinalIgnoreCase))
        {
            var parts = endpoint["Endpoint=".Length..].Split(';');
            return parts[0];
        }

        return endpoint;
    }
}
