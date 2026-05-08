using SchoolAccount.Integration.AcademiesApi.Models;
using SchoolAccount.Integration.DfESignIn;
using SchoolAccount.Integration.DfESignIn.Models;

namespace SchoolAccount.Kernel.Organisations;

public class TrustOrganisation(string ukrpn, string name) : IOrganisation
{
    public TrustOrganisation(OrganisationClaim claim)
        : this(claim.Ukprn!, claim.Name!) { }

    public TrustOrganisation(AcademyTrust trust)
        : this(trust.GiasData!.Ukprn!, trust.GiasData!.GroupName!)
    {
        TrustData = trust;
    }

    public string Ukrpn { get; } = ukrpn;
    public string Name { get; } = name;
    public AcademyTrust? TrustData { get; }
}
