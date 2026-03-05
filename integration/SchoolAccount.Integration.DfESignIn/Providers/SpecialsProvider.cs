using Microsoft.FeatureManagement;
using SchoolAccount.Integration.DfESignIn.Interfaces;

namespace SchoolAccount.Integration.DfESignIn.Providers;

public class SpecialsProvider(IFeatureManager featureManager) : IProvider
{
    public bool IsProvider(OrganisationClaim organisation)
    {
        return organisation.Type?.Id
            is EstablishmentType.CommunitySpecialSchool
            or EstablishmentType.FoundationSpecialSchool
            or EstablishmentType.OtherIndependentSpecialSchool
            or EstablishmentType.NonMaintainedSpecialSchool;
    }

    public async Task<bool> CanAccess()
    {
        return await featureManager.IsEnabledAsync("AllowedSchools.Specials");
    }
}