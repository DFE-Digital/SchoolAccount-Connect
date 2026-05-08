using SchoolAccount.Integration.DfESignIn.Interfaces;
using SchoolAccount.Integration.DfESignIn.Models;

namespace SchoolAccount.Integration.DfESignIn.Providers;

public class PreSixteenProvider : IProvider
{
    public bool IsProvider(OrganisationClaim organisation)
    {
        return organisation.Type?.Id
            is EstablishmentType.AcademyConverter
                or EstablishmentType.AcademySpecialConverter
                or EstablishmentType.AcademySponsorLed
                or EstablishmentType.AcademyAlternativeProvisionConverter
                or EstablishmentType.AcademySpecialSponsorLed
                or EstablishmentType.AcademyAlternativeProvisionSponsorLed;
    }

    public Task<bool> CanAccess()
    {
        return Task.FromResult(true);
    }
}
