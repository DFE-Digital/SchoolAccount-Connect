using System.Collections.ObjectModel;
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
        Data = organisation;
        
        PhaseOfEducation = organisation.PhaseOfEducation;

        Conditions = new Collection<EstablishmentCondition>(
            organisation.Conditions?
                .Where(x => x.Value is not null)
                .Select(x => new EstablishmentCondition(x.Identifier, x.Value!))
                .ToList()
            ?? []);
    }

    public string Ukrpn { get; } = ukrpn;
    public string Name { get; } = name;
    
    public object? Data { get; }
    
    public IdName<int>? PhaseOfEducation { get; set; }

    public Collection<EstablishmentCondition> Conditions { get; init; } = [];
}

public record EstablishmentCondition(string Identifier, object Value); 
