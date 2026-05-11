using System.Diagnostics.CodeAnalysis;
using SchoolAccount.Integration.DfESignIn.Common;

namespace SchoolAccount.Integration.DfESignIn.Models;

[SuppressMessage("Design", "CA1056:URI-like properties should not be strings")]
public class OrganisationClaim : IdName<Guid>
{
    public string LegalName { get; init; } = null!;

    public OrganisationCategoryClaim? Category { get; init; }
    public OrganisationEstablishmentTypeClaim? Type { get; init; }

    public string? Urn { get; init; }
    public string? Uid { get; init; }
    public string? Upin { get; init; }
    public string? UkPrn { get; init; }
    public string? EstablishmentNumber { get; set; }

    public OrganisationStateClaim? Status { get; init; }

    public DateTime? ClosedOn { get; init; }
    public DateTime? OpenedOn { get; init; }

    public string? Address { get; init; }
    public string? Telephone { get; init; }

    public IdName<string>? Region { get; init; }
    public IdCodeName<Guid, string>? LocalAuthority { get; init; }
    public IdName<int>? PhaseOfEducation { get; init; }

    public int? StatutoryLowAge { get; init; }
    public int? StatutoryHighAge { get; init; }

    public string? LegacyId { get; init; }
    public string? CompanyRegistrationNumber { get; init; }

    public string? SourceSystem { get; init; }

    public string? ProviderTypeName { get; init; }
    public int? ProviderTypeCode { get; init; }
    public string? GiasProviderType { get; init; }
    public int? GiasStatus { get; init; }
    public string? PimsProviderType { get; init; }
    public int? PimsProviderTypeCode { get; init; }
    public string? PimsStatusName { get; init; }
    public string? PimsStatus { get; init; }
    public string? GiasStatusName { get; init; }

    public string? MasterProviderStatusName { get; init; }
    public int? MasterProviderStatusCode { get; init; }

    public string? DistrictAdministrativeName { get; init; }
    public string? DistrictAdministrativeCode { get; init; }

    public string? IsOnApar { get; init; }
}
