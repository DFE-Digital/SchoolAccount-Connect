using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Integration.DfESignIn;
using SchoolAccount.Integration.DfESignIn.Interfaces;
using SchoolAccount.Integration.DfESignIn.Models;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Providers;

public class FallbackProvider(IFallbackProviderResolver fallbackProviderResolver) : IProvider
{
    public int Priority { get; } = int.MaxValue;

    public bool IsProvider(OrganisationClaim organisation)
    {
        return !string.IsNullOrEmpty(organisation.UkPrn)
            && fallbackProviderResolver.TryGetProvider(organisation.UkPrn, out _);
    }

    public async Task<bool> CanAccess(OrganisationClaim? organisation)
    {
        return !string.IsNullOrEmpty(organisation?.UkPrn)
            && fallbackProviderResolver.TryGetProvider(organisation.UkPrn, out var provider)
            && provider.HasAccess;
    }
}
