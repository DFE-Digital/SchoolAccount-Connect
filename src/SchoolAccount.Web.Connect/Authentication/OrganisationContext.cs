using SchoolAccount.Application.Resolvers.Interfaces;
using SchoolAccount.Integration.DfESignIn;
using SchoolAccount.Integration.DfESignIn.Interfaces;
using SchoolAccount.Integration.DfESignIn.Providers;
using SchoolAccount.Kernel;
using SchoolAccount.Web.Connect.Extensions;

namespace SchoolAccount.Web.Connect.Authentication;

public class OrganisationContext(
    IHttpContextAccessor contextAccessor,
    IProviderResolver providerResolver,
    IOrganisationResolver organisationResolver
) : IOrganisationContext
{
    private SchoolType? _schoolType;
    private IProvider? _provider;
    private IOrganisation? _organisation;

    private OrganisationClaim? Claim { get; } = contextAccessor.GetOrganisation();

    public bool IsValid => Claim is not null;
    public bool IsAuthenticated => IsValid && Provider is not NullProvider;

    public SchoolType Type => _schoolType ??= DetermineSchoolType();

    public EstablishmentType Establishment => Claim?.Type?.Id ?? EstablishmentType.Undeclared;

    public OrganisationCategory Category => Claim?.Category?.Id ?? OrganisationCategory.Undeclared;

    public IProvider Provider => _provider ??= providerResolver.Resolve(Claim);

    public IOrganisation Organisation => _organisation ??= organisationResolver.Resolve(Claim);

    private SchoolType DetermineSchoolType()
    {
        return Claim?.Type?.Id switch
        {
            EstablishmentType.AcademyConverter
            or EstablishmentType.AcademySponsorLed
            or EstablishmentType.AcademyAlternativeProvisionConverter
            or EstablishmentType.AcademyAlternativeProvisionSponsorLed
            or EstablishmentType.FreeSchools
            or EstablishmentType.FreeSchoolsAlternativeProvision => SchoolType.Academy,

            EstablishmentType.AcademySpecialConverter
            or EstablishmentType.AcademySpecialSponsorLed
            or EstablishmentType.FreeSchoolsSpecial => SchoolType.AcademySpecial,

            _ => SchoolType.Unknown,
        };
    }
}
