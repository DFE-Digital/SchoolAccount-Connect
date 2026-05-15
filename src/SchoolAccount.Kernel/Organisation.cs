using System.Collections.ObjectModel;
using SchoolAccount.Integration.AcademiesApi.Models;
using SchoolAccount.Integration.DfESignIn.Common;
using SchoolAccount.Integration.DfESignIn.Models;
using SchoolAccount.Kernel.Conditions.Interface;
using SchoolAccount.Kernel.Extensions;

namespace SchoolAccount.Kernel;

public record OrganisationCondition(string Identifier, object? Value); 

public class Organisation
{
    public static Organisation CreateFromClaim(OrganisationClaim claim)
    {
        return new Organisation
        {
            UkPrn = claim.UkPrn ?? throw new ArgumentException(nameof(UkPrn)),
            Name = claim.Name ?? throw new ArgumentException(nameof(Name)),
            LegalName = claim.LegalName ?? throw new ArgumentException(nameof(LegalName)),

            Address = claim.Address,
            Telephone = claim.Telephone,

            Establishment = claim.Type?.Id ?? EstablishmentType.Undeclared,
            Category = claim.Category?.Id ?? OrganisationCategory.Undeclared,
            StatutoryAges = new IntRange(claim.StatutoryHighAge ?? int.MaxValue, claim.StatutoryLowAge ?? 0),
            PhaseOfEducation = claim.PhaseOfEducation,
            Region = claim.Region,
            LocalAuthority = claim.LocalAuthority,

            Conditions = []
        };
    }

    public static async Task<Organisation> CreateFromAcademyOrganisation(AcademyOrganisation establishment,
        IConditionMapperResolver conditionMapperResolver)
    {

        if (!int.TryParse(establishment.EstablishmentType?.Code, out int establishmentType))
        {
            throw new ArgumentException(nameof(EstablishmentType));
        }

        return new Organisation
        {
            UkPrn = establishment.Ukprn,
            Name = establishment.EstablishmentName,
            LegalName = establishment.EstablishmentName,

            Address = establishment.Address?.ToString(),
            Telephone = establishment.TelephoneNumber,

            Establishment = (EstablishmentType)establishmentType,
            Category = OrganisationCategory.MultiAcademyTrust,
            StatutoryAges = new IntRange(
                establishment.StatutoryHighAge.ToIntOrDefault(int.MaxValue),
                establishment.StatutoryLowAge.ToIntOrDefault(0)),
            PhaseOfEducation = establishment.PhaseOfEducation is not null
                ? new IdName<int>
                {
                    Id = establishment.PhaseOfEducation.Code.ToIntOrDefault(-1),
                    Name = establishment.PhaseOfEducation.Name
                }
                : null,
            Region = establishment.Address is not null
                ? new IdName<string>
                {
                    Name = establishment.Address.Locality
                }
                : null,
            LocalAuthority = new IdCodeName<Guid, string>
            {
                Code = establishment.LocalAuthorityCode,
                Name = establishment.LocalAuthorityName
            },

            Conditions = await conditionMapperResolver.Resolve(establishment)
        };
    }

    public static async Task<Organisation> CreateFromAcademyTrust(AcademyTrust trust, IConditionMapperResolver conditionMapperResolver)
    {
        if (!EnumExtensions.TryParseFlexible<OrganisationCategory>(trust.GiasData?.GroupType, out var groupType))
        {
            throw new ArgumentException(nameof(trust.GiasData.GroupType));
        }
        
        var children = new Collection<Organisation>();

        foreach (var establishment in trust.Establishments)
        {
            children.Add(await CreateFromAcademyEstablishment(establishment, conditionMapperResolver));
        }

        return new Organisation
        {
            UkPrn = trust.GiasData?.Ukprn ?? throw new ArgumentException(nameof(UkPrn)),
            Name = trust.GiasData?.GroupName ?? throw new ArgumentException(nameof(UkPrn)),
            LegalName = trust.GiasData.GroupName,

            Address = trust.GiasData.GroupContactAddress?.ToString(),

            Establishment = EstablishmentType.Undeclared,
            Category = groupType,
            
            Children = children
        };
    }

    public static async Task<Organisation> CreateFromAcademyEstablishment(AcademyEstablishment establishment,
        IConditionMapperResolver conditionMapperResolver)
    {
        if (!int.TryParse(establishment.EstablishmentType?.Code, out int establishmentType))
        {
            throw new ArgumentException(nameof(EstablishmentType));
        }

        return new Organisation
        {
            UkPrn = establishment.Ukprn,
            Name = establishment.EstablishmentName,
            LegalName = establishment.EstablishmentName,

            Address = establishment.Address?.ToString(),
            Telephone = establishment.TelephoneNumber,

            Establishment = (EstablishmentType)establishmentType,
            Category = OrganisationCategory.MultiAcademyTrust,

            StatutoryAges = new IntRange(
                establishment.StatutoryHighAge.ToIntOrDefault(int.MaxValue),
                establishment.StatutoryLowAge.ToIntOrDefault(0)),
            PhaseOfEducation = establishment.PhaseOfEducation is not null
                ? new IdName<int>()
                {
                    Id = establishment.PhaseOfEducation.Code.ToIntOrDefault(-1),
                    Name = establishment.PhaseOfEducation.Name
                }
                : null,
            Region = establishment.Address is not null
                ? new IdName<string>()
                {
                    Name = establishment.Address.Locality
                }
                : null,
            LocalAuthority = new IdCodeName<Guid, string>()
            {
                Code = establishment.LocalAuthorityCode,
                Name = establishment.LocalAuthorityName
            },

            Conditions = await conditionMapperResolver.Resolve(establishment)
        };
    }

    public string UkPrn { get; init; } = null!;
    public string Name { get; init; } = null!;
    public string LegalName { get; init; } = null!;
    
    public string? Address { get; init; }
    public string? Telephone { get; init; }
    
    public EstablishmentType Establishment { get; init; }
    public OrganisationCategory Category { get; init; }
    
    public IntRange? StatutoryAges { get; set; }
    public IdName<int>? PhaseOfEducation { get; set; }
    public IdName<string>? Region { get; init; }
    public IdCodeName<Guid, string>? LocalAuthority { get; init; }

    public Collection<Organisation>? Children { get; init; }
    
    public Collection<OrganisationCondition>? Conditions { get; init; }
}