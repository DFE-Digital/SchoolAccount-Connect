using SchoolAccount.Integration.DfESignIn.Interfaces;

namespace SchoolAccount.Integration.DfESignIn.Providers;

public class FreeSchoolProvider : IProvider
{
    public bool IsProvider(OrganisationClaim organisation)
    {
        return organisation.Type?.Id
            is EstablishmentType.FreeSchools
                or EstablishmentType.FreeSchoolsAlternativeProvision
                or EstablishmentType.FreeSchoolsSpecial;
    }

    public Task<bool> CanAccess()
    {
        return Task.FromResult(true);
    }
}
