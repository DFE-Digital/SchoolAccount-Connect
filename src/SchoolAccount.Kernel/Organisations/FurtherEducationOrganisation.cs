using SchoolAccount.Integration.DfESignIn;
using SchoolAccount.Integration.DfESignIn.Models;

namespace SchoolAccount.Kernel.Organisations;

public class FurtherEducationOrganisation(string ukrpn, string name) : IOrganisation
{
    public FurtherEducationOrganisation(OrganisationClaim claim)
        : this(claim.UkPrn!, claim.Name!) 
    { }
    
    public FurtherEducationOrganisation(Organisation organisation)
        : this(organisation.UkPrn, organisation.Name) 
    { }

    public string Ukrpn { get; } = ukrpn;
    public string Name { get; } = name;
}