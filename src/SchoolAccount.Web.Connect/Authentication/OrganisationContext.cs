using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using SchoolAccount.Application.Resolvers.Interfaces;
using SchoolAccount.Integration.AcademiesApi.Models;
using SchoolAccount.Integration.DfESignIn;
using SchoolAccount.Integration.DfESignIn.Interfaces;
using SchoolAccount.Integration.DfESignIn.Models;
using SchoolAccount.Integration.DfESignIn.Providers;
using SchoolAccount.Kernel;
using SchoolAccount.Kernel.Organisations;
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


    private Organisation? Data
    {
        get
        {
            if (contextAccessor.HttpContext?.Session.GetString(SessionKeyConstants.OrgType) ==
                SessionKeyConstants.OrgTypeAcademy)
            {
                var obj = contextAccessor.HttpContext.Session.GetString(SessionKeyConstants.OrgSelected);

                if (!string.IsNullOrEmpty(obj))
                {
                    var acd = JsonSerializer.Deserialize<AcademyOrganisation>(obj);
                    return acd is not null ? new Organisation(acd) : null;
                }
            }
            
            if (contextAccessor.HttpContext?.Session.GetString(SessionKeyConstants.OrgType) ==
                SessionKeyConstants.OrgTypeTrust)
            {
                var obj = contextAccessor.HttpContext.Session.GetString(SessionKeyConstants.OrgSelected);

                if (!string.IsNullOrEmpty(obj))
                {
                    var tru = JsonSerializer.Deserialize<AcademyTrust>(obj);
                    return tru is not null ? new Organisation(tru) : null;
                }
            }
            
            var claim = contextAccessor.GetOrganisation();
            return claim is not null ? new Organisation(claim) : null;
        }
    }

    public bool IsAuthorised => (IsDsiDetermined || IsUserDeclared) && Provider is not NullProvider;

    public SchoolType Type => _schoolType ??= DetermineSchoolType();

    public EstablishmentType Establishment => Data?.Establishment ?? EstablishmentType.Undeclared;

    public OrganisationCategory Category => Data?.Category ?? OrganisationCategory.Undeclared;

    public IProvider Provider
    {
        get
        {
            return contextAccessor.HttpContext?.Session.GetString(SessionKeyConstants.OrgType) switch
            {
                SessionKeyConstants.OrgTypeAcademy => new PreSixteenProvider(),
                SessionKeyConstants.OrgTypeTrust => new TrustProvider(),
                _ => providerResolver.Resolve(contextAccessor.GetOrganisation())
            };
        }
    }

    public IOrganisation Organisation
    {
        get
        {
            return contextAccessor.HttpContext?.Session.GetString(SessionKeyConstants.OrgType) switch
            {
                SessionKeyConstants.OrgTypeAcademy => new EstablishmentOrganisation(Data!),
                SessionKeyConstants.OrgTypeTrust => new TrustOrganisation(Data!),
                _ => organisationResolver.Resolve(contextAccessor.GetOrganisation())
            };
        }
    }

    private SchoolType DetermineSchoolType()
    {
        return Data?.Category switch
        {
            OrganisationCategory.SingleAcademyTrust => SchoolType.SingleAcademyTrust,
            OrganisationCategory.MultiAcademyTrust => SchoolType.MultiAcademyTrust,
            _ => Data?.Establishment switch
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
