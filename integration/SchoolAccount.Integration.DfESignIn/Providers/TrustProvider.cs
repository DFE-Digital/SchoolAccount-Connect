using SchoolAccount.Integration.DfESignIn.Interfaces;
using SchoolAccount.Integration.DfESignIn.Models;

namespace SchoolAccount.Integration.DfESignIn.Providers;

public class TrustProvider : IProvider
{
    public int Priority { get; } = 2;

    public bool IsProvider(OrganisationClaim organisation)
    {
        return organisation.Category?.Id
            is OrganisationCategory.SingleAcademyTrust
                or OrganisationCategory.MultiAcademyTrust;
    }

    public Task<bool> CanAccess(OrganisationClaim? organisation)
    {
        return Task.FromResult(true);
    }
}
