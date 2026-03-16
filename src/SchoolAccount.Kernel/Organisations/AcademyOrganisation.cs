using SchoolAccount.Integration.DfESignIn;

namespace SchoolAccount.Kernel.Organisations;

public class AcademyOrganisation(string ukrpn, string name) : IOrganisation
{
    public AcademyOrganisation(OrganisationClaim claim)
        : this(claim.Ukprn!, claim.Name!) { }

    public string Ukrpn { get; } = ukrpn;
    public string Name { get; } = name;
}
