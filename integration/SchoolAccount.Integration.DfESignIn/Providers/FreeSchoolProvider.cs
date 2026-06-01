using SchoolAccount.Integration.DfESignIn.Interfaces;
using SchoolAccount.Integration.DfESignIn.Models;

namespace SchoolAccount.Integration.DfESignIn.Providers;

public class FreeSchoolProvider : IProvider
{
    public int Priority { get; } = 1;

    public bool IsProvider(OrganisationClaim organisation)
    {
        return organisation.Type?.Id
            is EstablishmentType.FreeSchools
                or EstablishmentType.FreeSchoolsAlternativeProvision
                or EstablishmentType.FreeSchoolsSpecial;
    }

    public Task<bool> CanAccess(OrganisationClaim? organisation)
    {
        return Task.FromResult(true);
    }
}
