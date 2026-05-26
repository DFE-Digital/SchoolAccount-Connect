using SchoolAccount.Integration.DfESignIn.Interfaces;

namespace SchoolAccount.Integration.DfESignIn.Providers;

public class PreSixteenProvider : IProvider
{
    public int Priority { get; } = 1;

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

    public Task<bool> CanAccess(OrganisationClaim? organisation)
    {
        return Task.FromResult(true);
    }
}
