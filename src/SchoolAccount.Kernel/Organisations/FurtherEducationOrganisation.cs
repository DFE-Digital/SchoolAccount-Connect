using SchoolAccount.Integration.DfESignIn;

namespace SchoolAccount.Kernel.Organisations;

public class FurtherEducationOrganisation(string ukrpn, string name) : IOrganisation
{
    public FurtherEducationOrganisation(OrganisationClaim claim)
        : this(claim.Ukprn!, claim.Name!) { }

    public string Ukrpn { get; } = ukrpn;
    public string Name { get; } = name;
}