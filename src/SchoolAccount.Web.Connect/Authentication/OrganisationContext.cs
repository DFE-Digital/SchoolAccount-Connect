using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using SchoolAccount.Application.Resolvers.Interfaces;
using SchoolAccount.Integration.AcademiesApi.Models;
using SchoolAccount.Integration.DfESignIn.Interfaces;
using SchoolAccount.Integration.DfESignIn.Models;
using SchoolAccount.Integration.DfESignIn.Providers;
using SchoolAccount.Kernel;
using SchoolAccount.Kernel.Conditions.Interface;
using SchoolAccount.Kernel.Organisations;
using SchoolAccount.Web.Connect.Extensions;

namespace SchoolAccount.Web.Connect.Authentication;

[SuppressMessage("Maintainability", "CA1508:Avoid dead conditional code")]
public class OrganisationContext(
    IHttpContextAccessor contextAccessor,
    IProviderResolver providerResolver,
    IOrganisationResolver organisationResolver,
    IConditionMapperResolver conditionMapperResolver
) : IOrganisationContext
{
    public bool IsDsiDetermined => contextAccessor.HttpContext?.User.FindFirst("organisation") is not null;
    public bool IsUserDeclared => contextAccessor.HttpContext?.Session.Keys.Contains(SessionKeyConstants.OrgType) == true;

    private async Task<Organisation?> Populate()
    {
        Organisation? organisation = null;

        if (contextAccessor.HttpContext?.Session.GetString(SessionKeyConstants.ComputedOrg) is { } storedOrg 
            && !string.IsNullOrEmpty(storedOrg))
        {
            return JsonSerializer.Deserialize<Organisation>(storedOrg);
        }
        
        if (contextAccessor.HttpContext?.Session.GetString(SessionKeyConstants.OrgType) ==
            SessionKeyConstants.OrgTypeAcademy)
        {
            var obj = contextAccessor.HttpContext.Session.GetString(SessionKeyConstants.OrgSelected);

            if (!string.IsNullOrEmpty(obj))
            {
                var acd = JsonSerializer.Deserialize<AcademyOrganisation>(obj);
                organisation = acd is not null 
                    ? await Kernel.Organisation.CreateFromAcademyOrganisation(acd, conditionMapperResolver) 
                    : null;
            }
        }
            
        if (contextAccessor.HttpContext?.Session.GetString(SessionKeyConstants.OrgType) ==
            SessionKeyConstants.OrgTypeTrust)
        {
            var obj = contextAccessor.HttpContext.Session.GetString(SessionKeyConstants.OrgSelected);

            if (!string.IsNullOrEmpty(obj))
            {
                var tru = JsonSerializer.Deserialize<AcademyTrust>(obj);
                organisation = tru is not null 
                    ? await Kernel.Organisation.CreateFromAcademyTrust(tru, conditionMapperResolver) 
                    : null;
            }
        }

        if (organisation is null)
        {
            var claim = contextAccessor.GetOrganisation();
            organisation = claim is not null 
                ? Kernel.Organisation.CreateFromClaim(claim) 
                : null;
        }

        if (organisation is not null)
        {
            contextAccessor.HttpContext?.Session.SetString(
                SessionKeyConstants.ComputedOrg,
                JsonSerializer.Serialize(organisation));
        }
        
        return organisation;
    }
    
    private Organisation? Data
    {
        get
        {
            return field ??= Populate().GetAwaiter().GetResult();
        }
    }

    public bool IsAuthorised => (IsDsiDetermined || IsUserDeclared) && Provider is not NullProvider;

    public Dictionary<string, SchoolType> Type => field ??= DetermineSchoolType();

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
        [SuppressMessage("Design", "CA1065:Do not raise exceptions in unexpected locations")]
        get
        {
            return contextAccessor.HttpContext?.Session.GetString(SessionKeyConstants.OrgType) switch
            {
                SessionKeyConstants.OrgTypeAcademy => new EstablishmentOrganisation(Data!),
                SessionKeyConstants.OrgTypeTrust => TrustOrganisation.CreateFromOrganisation(Data!),
                _ => organisationResolver.Resolve(contextAccessor.GetOrganisation())
            };
        }
    }

    public static SchoolType ComputeSchoolType(IOrganisation? organisation)
    {
        return organisation?.Category switch
        {
            OrganisationCategory.SingleAcademyTrust => SchoolType.SingleAcademyTrust,
            OrganisationCategory.MultiAcademyTrust => SchoolType.MultiAcademyTrust,
            _ => organisation?.Establishment switch
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
    
    private Dictionary<string, SchoolType> DetermineSchoolType()
    {
        var response = new Dictionary<string, SchoolType>
        {
            { Organisation.Ukrpn, ComputeSchoolType(Organisation) }
        };

        if (Organisation is TrustOrganisation trust)
        {
            foreach (var establishment in trust.Establishments)
            {
                response.Add(establishment.Ukrpn, ComputeSchoolType(establishment));
            }    
        }
        
        return response;
    }
}
