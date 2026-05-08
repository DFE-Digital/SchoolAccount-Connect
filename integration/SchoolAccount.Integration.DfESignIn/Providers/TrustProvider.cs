using SchoolAccount.Integration.DfESignIn.Interfaces;
using SchoolAccount.Integration.DfESignIn.Models;

namespace SchoolAccount.Integration.DfESignIn.Providers;

public class TrustProvider : IProvider
{
    public bool IsProvider(OrganisationClaim organisation)
    {
        return organisation.Category?.Id
            is OrganisationCategory.SingleAcademyTrust
                or OrganisationCategory.MultiAcademyTrust;
    }

    public Task<bool> CanAccess()
    {
        return Task.FromResult(true);
    }
}
