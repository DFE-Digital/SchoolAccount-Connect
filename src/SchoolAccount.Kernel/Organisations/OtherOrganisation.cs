using SchoolAccount.Integration.DfESignIn;

namespace SchoolAccount.Kernel.Organisations;

public class OtherOrganisation(string ukrpn, string name) : IOrganisation
{
    public OtherOrganisation(OrganisationClaim claim) : this(claim.Ukprn!, claim.Name!)
    { }

    public string Ukrpn { get; } = ukrpn;
    public string Name { get; } = name;
}