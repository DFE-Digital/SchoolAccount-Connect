using Microsoft.AspNetCore.Authorization;
using SchoolAccount.Integration.DfESignIn.Interfaces;

namespace SchoolAccount.Integration.DfESignIn.Requirements;

public class ProviderRequirement(
    params Type[] allowedProviders
) : IAuthorizationRequirement
{
    public IReadOnlyCollection<Type> AllowedProviders { get; } = allowedProviders;

    public bool IsProviderAllowed(IProvider provider)
    {
        return AllowedProviders.Count == 0 || AllowedProviders.Any(t => t.IsInstanceOfType(provider));
    }
}