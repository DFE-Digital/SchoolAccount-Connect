using Microsoft.FeatureManagement;
using SchoolAccount.Integration.DfESignIn.Interfaces;

namespace SchoolAccount.Integration.DfESignIn.Providers;

public class LamsProvider(IFeatureManager featureManager) : IProvider
{
    public bool IsProvider(OrganisationClaim organisation)
    {
        return organisation.Type?.Id
            is EstablishmentType.VoluntaryAidedSchool
            or EstablishmentType.LaNurserySchool
            or EstablishmentType.PupilReferralUnit
            or EstablishmentType.CommunitySchool
            or EstablishmentType.FoundationSchool
            or EstablishmentType.VoluntaryControlledSchool;
    }

    public async Task<bool> CanAccess()
    {
        return await featureManager.IsEnabledAsync("AllowedSchools.Lams");
    }
}