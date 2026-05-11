using SchoolAccount.Integration.DfESignIn;
using SchoolAccount.Integration.DfESignIn.Models;

namespace SchoolAccount.Kernel.Organisations;

public class EstablishmentOrganisation(string ukrpn, string name) : IOrganisation
{
    public EstablishmentOrganisation(OrganisationClaim claim)
        : this(claim.UkPrn!, claim.Name!) 
    { }
    
    public EstablishmentOrganisation(Organisation organisation) 
        : this(organisation.UkPrn, organisation.Name) 
    { }

    public string Ukrpn { get; } = ukrpn;
    public string Name { get; } = name;
}
