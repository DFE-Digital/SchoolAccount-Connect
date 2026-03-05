using System.Text.Json;
using SchoolAccount.Integration.DfESignIn;
using SchoolAccount.Integration.DfESignIn.Interfaces;
using SchoolAccount.Integration.DfESignIn.Providers;
using SchoolAccount.Kernel;

namespace SchoolAccount.Web.Connect.Authentication;

public class OrganisationContext : IOrganisationContext
{
    private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };
    private SchoolType? _schoolType;
    private IProvider? _provider;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IProviderResolver _providerResolver;

    public OrganisationContext(IHttpContextAccessor contextAccessor, IProviderResolver providerResolver)
    {
        _contextAccessor = contextAccessor;
        _providerResolver = providerResolver;

        var organisation = _contextAccessor.HttpContext?.User.FindFirst(ClaimConstants.Organisation)?.Value;
        if (!string.IsNullOrEmpty(organisation))
        {
            Organisation = JsonSerializer.Deserialize<OrganisationClaim>(organisation, _options);
        }
    }

    public OrganisationClaim? Organisation { get; }

    public bool IsValid => Organisation is not null;
    public bool IsAuthenticated => IsValid && Provider is not NullProvider; 
    public string Ukrpn => Organisation?.Ukprn!;
    public string Name => Organisation?.Name!;
    public SchoolType Type => _schoolType ??= DetermineSchoolType();
    public EstablishmentType Establishment => Organisation?.Type?.Id ?? EstablishmentType.Undeclared;
    public OrganisationCategory Category => Organisation?.Category?.Id ?? OrganisationCategory.Undeclared;

    public IProvider Provider => _provider ??= _providerResolver.Resolve(Organisation);
    
    private SchoolType DetermineSchoolType()
    {
        return Organisation?.Type?.Id switch
        {
            EstablishmentType.AcademyConverter
                or EstablishmentType.AcademySponsorLed
                or EstablishmentType.AcademyAlternativeProvisionConverter
                or EstablishmentType.AcademyAlternativeProvisionSponsorLed
                or EstablishmentType.FreeSchools
                or EstablishmentType.FreeSchoolsAlternativeProvision
                => SchoolType.Academy,
                
            EstablishmentType.AcademySpecialConverter
                or EstablishmentType.AcademySpecialSponsorLed
                or EstablishmentType.FreeSchoolsSpecial
                => SchoolType.AcademySpecial,
            
            _ => SchoolType.Unknown
        };
    }
}