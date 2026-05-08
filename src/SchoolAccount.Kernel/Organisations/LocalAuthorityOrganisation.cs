using SchoolAccount.Integration.DfESignIn;
using SchoolAccount.Integration.DfESignIn.Models;

namespace SchoolAccount.Kernel.Organisations;

public class LocalAuthorityOrganisation(string ukrpn, string name) : IOrganisation
{
    public LocalAuthorityOrganisation(OrganisationClaim claim)
        : this(claim.Ukprn!, claim.Name!) { }

    public string Ukrpn { get; } = ukrpn;
    public string Name { get; } = name;
}
