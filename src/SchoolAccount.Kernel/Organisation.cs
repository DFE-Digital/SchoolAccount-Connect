using System.Collections.ObjectModel;
using SchoolAccount.Integration.AcademiesApi.Models;
using SchoolAccount.Integration.DfESignIn.Common;
using SchoolAccount.Integration.DfESignIn.Models;
using SchoolAccount.Kernel.Extensions;

namespace SchoolAccount.Kernel;

public record OrganisationCondition(string Identifier, object? Value); 

public class Organisation
{
    public Organisation(OrganisationClaim claim)
    {
        UkPrn = claim.UkPrn ?? throw new ArgumentException(nameof(UkPrn));
        Name = claim.Name ?? throw new ArgumentException(nameof(Name));
        LegalName = claim.LegalName ?? throw new ArgumentException(nameof(LegalName));

        Address = claim.Address;
        Telephone = claim.Telephone;
        
        Establishment = claim.Type?.Id ?? EstablishmentType.Undeclared;
        Category = claim.Category?.Id ?? OrganisationCategory.Undeclared;
        StatutoryAges = new IntRange(claim.StatutoryHighAge ?? int.MaxValue, claim.StatutoryLowAge ?? 0);
        PhaseOfEducation = claim.PhaseOfEducation;
        Region = claim.Region;
        LocalAuthority = claim.LocalAuthority;
    }

    public Organisation(AcademyOrganisation establishment)
    {
        UkPrn = establishment.Ukprn;
        Name = establishment.EstablishmentName;
        LegalName = establishment.EstablishmentName;

        Address = establishment.Address?.ToString();
        Telephone = establishment.TelephoneNumber;

        if (int.TryParse(establishment.EstablishmentType?.Code, out int establishmentType))
        {
            Establishment = (EstablishmentType)establishmentType;
        }
        else
        {
            throw new ArgumentException(nameof(EstablishmentType));
        }
        Category = OrganisationCategory.MultiAcademyTrust;
        StatutoryAges = new IntRange(
            establishment.StatutoryHighAge.ToIntOrDefault(int.MaxValue), 
            establishment.StatutoryLowAge.ToIntOrDefault(0));
        PhaseOfEducation = establishment.PhaseOfEducation is not null
            ? new IdName<int>()
            {
                Id = establishment.PhaseOfEducation.Code.ToIntOrDefault(-1),
                Name = establishment.PhaseOfEducation.Name
            }
            : null;
        Region = establishment.Address is not null
            ? new IdName<string>()
            {
                Name = establishment.Address.Locality
            }
            : null;
        LocalAuthority = new IdCodeName<Guid, string>()
        {
            Code = establishment.LocalAuthorityCode,
            Name = establishment.LocalAuthorityName
        };

        Conditions =
        [
            new("Census.NumberOfBoys", establishment.Census?.NumberOfBoys),
            new("Census.NumberOfGirls", establishment.Census?.NumberOfGirls),
            new("Census.NumberOfPupils", establishment.Census?.NumberOfPupils),
            new("SmartData.RiskRatingNum", establishment.SmartData?.RiskRatingNum),
            new("SmartData.ProbabilityOfDeclining", establishment.SmartData?.ProbabilityOfDeclining),
            new("SmartData.ProbabilityOfImproving", establishment.SmartData?.ProbabilityOfImproving),
            new("SmartData.ProbabilityOfStayingTheSame", establishment.SmartData?.ProbabilityOfStayingTheSame),
        ];
    }

    public Organisation(AcademyTrust trust)
    {
        UkPrn = trust.GiasData?.Ukprn ?? throw new ArgumentException(nameof(UkPrn));
        Name = LegalName = trust.GiasData?.GroupName ?? throw new ArgumentException(nameof(UkPrn));

        Address = trust.GiasData.GroupContactAddress?.ToString();

        Establishment = EstablishmentType.Undeclared;

        if (EnumExtensions.TryParseFlexible<OrganisationCategory>(trust.GiasData.GroupType, out var groupType))
        {
            Category = groupType;
        }
        else
        {
            throw new ArgumentException(nameof(trust.GiasData.GroupType));
        }

        Children = new Collection<Organisation>(
            trust.Establishments
                .Select(x => new Organisation(x))
                .ToList());
    }

    public Organisation(AcademyEstablishment establishment)
    {
        UkPrn = establishment.Ukprn;
        Name = establishment.EstablishmentName;
        LegalName = establishment.EstablishmentName;

        Address = establishment.Address?.ToString();
        Telephone = establishment.TelephoneNumber;

        if (int.TryParse(establishment.EstablishmentType?.Code, out int establishmentType))
        {
            Establishment = (EstablishmentType)establishmentType;
        }
        else
        {
            throw new ArgumentException(nameof(EstablishmentType));
        }
        Category = OrganisationCategory.MultiAcademyTrust;
        StatutoryAges = new IntRange(
            establishment.StatutoryHighAge.ToIntOrDefault(int.MaxValue), 
            establishment.StatutoryLowAge.ToIntOrDefault(0));
        PhaseOfEducation = establishment.PhaseOfEducation is not null
            ? new IdName<int>()
            {
                Id = establishment.PhaseOfEducation.Code.ToIntOrDefault(-1),
                Name = establishment.PhaseOfEducation.Name
            }
            : null;
        Region = establishment.Address is not null
            ? new IdName<string>()
            {
                Name = establishment.Address.Locality
            }
            : null;
        LocalAuthority = new IdCodeName<Guid, string>()
        {
            Code = establishment.LocalAuthorityCode,
            Name = establishment.LocalAuthorityName
        };

        Conditions =
        [
            new("Census.NumberOfBoys", establishment.Census?.NumberOfBoys),
            new("Census.NumberOfGirls", establishment.Census?.NumberOfGirls),
            new("Census.NumberOfPupils", establishment.Census?.NumberOfPupils),
            new("SmartData.RiskRatingNum", establishment.SmartData?.RiskRatingNum),
            new("SmartData.ProbabilityOfDeclining", establishment.SmartData?.ProbabilityOfDeclining),
            new("SmartData.ProbabilityOfImproving", establishment.SmartData?.ProbabilityOfImproving),
            new("SmartData.ProbabilityOfStayingTheSame", establishment.SmartData?.ProbabilityOfStayingTheSame),
        ];
    }
    
    public string UkPrn { get; init; }
    public string Name { get; init; }
    public string LegalName { get; init; }
    
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