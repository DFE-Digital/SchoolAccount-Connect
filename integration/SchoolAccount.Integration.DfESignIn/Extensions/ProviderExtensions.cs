using SchoolAccount.Integration.DfESignIn.Interfaces;
using SchoolAccount.Integration.DfESignIn.Providers;

namespace SchoolAccount.Integration.DfESignIn.Extensions;

public static class ProviderExtensions
{
    public static bool IsProviderAllowed(this IProvider provider, Type[] allowedProviders)
    {
        return provider is not NullProvider
            && (allowedProviders.Length == 0 || allowedProviders.Any(t => t.IsInstanceOfType(provider)));
    }
}
