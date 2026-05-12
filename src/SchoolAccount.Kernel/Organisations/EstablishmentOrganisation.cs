using SchoolAccount.Integration.DfESignIn;
using SchoolAccount.Integration.DfESignIn.Common;
using SchoolAccount.Integration.DfESignIn.Models;

namespace SchoolAccount.Kernel.Organisations;

public class EstablishmentOrganisation(string ukrpn, string name) : IOrganisation
{
    public EstablishmentOrganisation(OrganisationClaim claim)
        : this(claim.UkPrn!, claim.Name!) 
    { }

    public EstablishmentOrganisation(Organisation organisation)
        : this(organisation.UkPrn, organisation.Name)
    {
        PhaseOfEducation = organisation.PhaseOfEducation;
    }

    public string Ukrpn { get; } = ukrpn;
    public string Name { get; } = name;
    
    
    public IdName<int>? PhaseOfEducation { get; set; }
}
