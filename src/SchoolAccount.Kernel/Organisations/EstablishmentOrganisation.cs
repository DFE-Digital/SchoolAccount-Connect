using SchoolAccount.Integration.DfESignIn;

namespace SchoolAccount.Kernel.Organisations;

public class EstablishmentOrganisation(string ukrpn, string name) : IOrganisation
{
    public EstablishmentOrganisation(OrganisationClaim claim)
        : this(claim.Ukprn!, claim.Name!) { }

    public string Ukrpn { get; } = ukrpn;
    public string Name { get; } = name;
}
