using System.Diagnostics.CodeAnalysis;
using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Domain.Providers;
using SchoolAccount.Integration.DfESignIn;
using SchoolAccount.Integration.DfESignIn.Interfaces;

namespace SchoolAccount.Application.Providers;

public class FallbackProvider(IFallbackProviderResolver fallbackProviderResolver) : IProvider
{
    public int Priority { get; } = int.MaxValue;

    public bool IsProvider(OrganisationClaim organisation)
    {
        return !string.IsNullOrEmpty(organisation.Ukprn)
               && fallbackProviderResolver.TryGetProvider(organisation.Ukprn, out _);
    }

    public async Task<bool> CanAccess(OrganisationClaim? organisation)
    {
        return !string.IsNullOrEmpty(organisation?.Ukprn)
               && fallbackProviderResolver.TryGetProvider(organisation.Ukprn, out var provider)
               && provider.HasAccess;
    }
}