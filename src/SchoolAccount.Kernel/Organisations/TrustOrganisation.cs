using System.Collections.ObjectModel;
using Microsoft.VisualBasic;
using SchoolAccount.Integration.AcademiesApi.Models;
using SchoolAccount.Integration.DfESignIn;
using SchoolAccount.Integration.DfESignIn.Models;
using SchoolAccount.Kernel.Conditions.Interface;

namespace SchoolAccount.Kernel.Organisations;

public class TrustOrganisation : IOrganisation
{
    public static TrustOrganisation CreateFromClaim(OrganisationClaim claim)
    {
        return new TrustOrganisation
        {
            Ukrpn = claim.UkPrn ?? throw new ArgumentException(nameof(claim.UkPrn)),
            Name = claim.Name ?? throw new ArgumentException(nameof(claim.Name)),
            Establishment = claim.Type is not null
                ? claim.Type!.Id
                : EstablishmentType.Undeclared,
            Category = claim.Category is not null
                ? claim.Category!.Id
                : OrganisationCategory.Undeclared
        };
    }

    public static async Task<TrustOrganisation> CreateFromAcademyTrust(AcademyTrust trust, IConditionMapperResolver conditionMapperResolver)
    {
        var establishments = new Collection<EstablishmentOrganisation>();

        foreach (var establishment in trust.Establishments)
        {
            establishments.Add(
                new EstablishmentOrganisation( 
                    await Organisation.CreateFromAcademyEstablishment(establishment, conditionMapperResolver)));
        }
        
        return new TrustOrganisation
        {
            Ukrpn = trust.GiasData?.Ukprn ?? throw new ArgumentException(nameof(trust.GiasData.Ukprn)),
            Name = trust.GiasData?.GroupName ?? throw new ArgumentException(nameof(trust.GiasData.GroupName)),
            Category = OrganisationCategory.MultiAcademyTrust,
            Establishment = EstablishmentType.Undeclared,
            Establishments = establishments
        };
    }

    public static TrustOrganisation CreateFromOrganisation(Organisation organisation)
    {
        return new TrustOrganisation
        {
            Ukrpn = organisation.UkPrn ?? throw new ArgumentException(nameof(organisation.UkPrn)),
            Name = organisation.Name ?? throw new ArgumentException(nameof(organisation.Name)),
            Category =  organisation.Category,
            Establishment = organisation.Establishment,
            Establishments = organisation.Children?.Select(x => new EstablishmentOrganisation(x)).ToList() ?? []
        };
    }

    public string Ukrpn { get; init; } = null!;
    public string Name { get; init; } = null!;
    
    public EstablishmentType Establishment { get; init; }
    public OrganisationCategory Category { get; init; }
    
    public IReadOnlyCollection<EstablishmentOrganisation> Establishments { get; init; } = [];
}
