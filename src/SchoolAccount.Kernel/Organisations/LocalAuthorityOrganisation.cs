using SchoolAccount.Integration.DfESignIn;
using SchoolAccount.Integration.DfESignIn.Models;

namespace SchoolAccount.Kernel.Organisations;

public class LocalAuthorityOrganisation(string ukrpn, string name) : IOrganisation
{
    public LocalAuthorityOrganisation(OrganisationClaim claim)
        : this(claim.UkPrn!, claim.Name!) 
    { 
        Establishment = claim.Type is not null 
            ? claim.Type!.Id 
            : EstablishmentType.Undeclared;
        Category = claim.Category is not null
            ? claim.Category!.Id
            : OrganisationCategory.Undeclared;
    }

    public LocalAuthorityOrganisation(Organisation organisation)
        : this(organisation.UkPrn, organisation.Name)
    {
        Establishment = organisation.Establishment;
        Category = organisation.Category;
    }

    public string Ukrpn { get; } = ukrpn;
    public string Name { get; } = name;
    
    public EstablishmentType Establishment { get; init; }
    public OrganisationCategory Category { get; init; }
}
