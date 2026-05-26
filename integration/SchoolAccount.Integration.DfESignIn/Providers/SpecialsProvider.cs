using Microsoft.FeatureManagement;
using SchoolAccount.Integration.DfESignIn.Interfaces;

namespace SchoolAccount.Integration.DfESignIn.Providers;

public class SpecialsProvider(IFeatureManager featureManager) : IProvider
{
    public int Priority { get; } = 1;

    public bool IsProvider(OrganisationClaim organisation)
    {
        return organisation.Type?.Id
            is EstablishmentType.CommunitySpecialSchool
                or EstablishmentType.FoundationSpecialSchool
                or EstablishmentType.OtherIndependentSpecialSchool
                or EstablishmentType.NonMaintainedSpecialSchool;
    }

    public async Task<bool> CanAccess(OrganisationClaim? organisation)
    {
        return await featureManager.IsEnabledAsync("AllowedSchools.Specials");
    }
}
