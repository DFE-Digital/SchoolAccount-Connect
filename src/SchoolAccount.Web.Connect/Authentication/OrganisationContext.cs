using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using SchoolAccount.Application.Resolvers.Interfaces;
using SchoolAccount.Integration.AcademiesApi.Models;
using SchoolAccount.Integration.DfESignIn;
using SchoolAccount.Integration.DfESignIn.Interfaces;
using SchoolAccount.Integration.DfESignIn.Models;
using SchoolAccount.Integration.DfESignIn.Providers;
using SchoolAccount.Kernel;
using SchoolAccount.Web.Connect.Extensions;

namespace SchoolAccount.Web.Connect.Authentication;

[SuppressMessage("Maintainability", "CA1508:Avoid dead conditional code")]
public class OrganisationContext(
    IHttpContextAccessor contextAccessor,
    IProviderResolver providerResolver,
    IOrganisationResolver organisationResolver
) : IOrganisationContext
{
    private SchoolType? _schoolType;

    public bool IsDsiDetermined => contextAccessor.HttpContext?.User.FindFirst("organisation") is not null;
    public bool IsUserDeclared => contextAccessor.HttpContext?.Session.Keys.Contains(SessionKeyConstants.OrgType) == true;


    private OrganisationClaim? Claim => contextAccessor.GetOrganisation();

    private AcademyOrganisation? Academy
    {
        get
        {
            if (contextAccessor.HttpContext?.Session.GetString(SessionKeyConstants.OrgType) ==
                SessionKeyConstants.OrgTypeAcademy)
            {
                var obj = contextAccessor.HttpContext.Session.GetString(SessionKeyConstants.OrgSelected);

                if (!string.IsNullOrEmpty(obj))
                {
                    return JsonSerializer.Deserialize<AcademyOrganisation>(obj);
                }
            }

            return null;
        }
    }
    private AcademyTrust? Trust
    {
        get
        {
            if (contextAccessor.HttpContext?.Session.GetString(SessionKeyConstants.OrgType) ==
                SessionKeyConstants.OrgTypeTrust)
            {
                var obj = contextAccessor.HttpContext.Session.GetString(SessionKeyConstants.OrgSelected);

                if (!string.IsNullOrEmpty(obj))
                {
                    return JsonSerializer.Deserialize<AcademyTrust>(obj);
                }
            }

            return null;
        }
    }

    public bool IsAuthorised => (IsDsiDetermined || IsUserDeclared) && Provider is not NullProvider;

    public SchoolType Type => _schoolType ??= DetermineSchoolType();

    public EstablishmentType Establishment => Claim?.Type?.Id ?? EstablishmentType.Undeclared;

    public OrganisationCategory Category => Claim?.Category?.Id ?? OrganisationCategory.Undeclared;

    public IProvider Provider => providerResolver.Resolve(Claim, Academy, Trust);

    public IOrganisation Organisation => organisationResolver.Resolve(Claim, Academy, Trust);

    private SchoolType DetermineSchoolType()
    {
        return Claim?.Category?.Id switch
        {
            OrganisationCategory.SingleAcademyTrust => SchoolType.SingleAcademyTrust,
            OrganisationCategory.MultiAcademyTrust => SchoolType.MultiAcademyTrust,
            _ => Claim?.Type?.Id switch
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
            }
        };
    }
}
