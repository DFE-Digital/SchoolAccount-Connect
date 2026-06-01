using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using SchoolAccount.Application.Resolvers.Interfaces;
using SchoolAccount.Integration.AcademiesApi.Models;
using SchoolAccount.Integration.DfESignIn;
using SchoolAccount.Integration.DfESignIn.Extensions;
using SchoolAccount.Integration.DfESignIn.Interfaces;
using SchoolAccount.Integration.DfESignIn.Models;
using SchoolAccount.Integration.DfESignIn.Providers;
using SchoolAccount.Kernel;
using SchoolAccount.Kernel.Conditions.Interface;
using SchoolAccount.Kernel.Organisations;

namespace SchoolAccount.Web.Connect.Authentication;

public class OrganisationContext : IOrganisationContext
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IProviderResolver _providerResolver;
    private readonly IEnumerable<IProviderContextResolver> _providerContextResolvers;
    private readonly IOrganisationResolver _organisationResolver;
    private readonly IConditionMapperResolver _conditionMapperResolver;
    
    public bool IsDsiDetermined => _contextAccessor.HttpContext?.User.FindFirst("organisation") is not null;
    public bool IsUserDeclared => _contextAccessor.HttpContext?.Session.Keys.Contains(SessionKeyConstants.OrgType) == true;

    public OrganisationContext(
        IHttpContextAccessor contextAccessor,
        IProviderResolver providerResolver,
        IOrganisationResolver organisationResolver,
        IEnumerable<IProviderContextResolver> providerContextResolvers,
        IConditionMapperResolver conditionMapperResolver
    )
    {
        _contextAccessor = contextAccessor;
        _providerResolver = providerResolver;
        _organisationResolver = organisationResolver;
        _providerContextResolvers = providerContextResolvers;
        _conditionMapperResolver = conditionMapperResolver;
    }
    
    private async Task<Organisation?> Populate()
    {
        Organisation? organisation = null;

        if (_contextAccessor.HttpContext?.Session.GetString(SessionKeyConstants.ComputedOrg) is { } storedOrg 
            && !string.IsNullOrEmpty(storedOrg))
        {
            return JsonSerializer.Deserialize<Organisation>(storedOrg);
        }
        
        if (_contextAccessor.HttpContext?.Session.GetString(SessionKeyConstants.OrgType) ==
            SessionKeyConstants.OrgTypeAcademy)
        {
            var obj = _contextAccessor.HttpContext.Session.GetString(SessionKeyConstants.OrgSelected);

            if (!string.IsNullOrEmpty(obj))
            {
                var acd = JsonSerializer.Deserialize<AcademyOrganisation>(obj);
                organisation = acd is not null 
                    ? await Kernel.Organisation.CreateFromAcademyOrganisation(acd, _conditionMapperResolver) 
                    : null;
            }
        }
            
        if (_contextAccessor.HttpContext?.Session.GetString(SessionKeyConstants.OrgType) ==
            SessionKeyConstants.OrgTypeTrust)
        {
            var obj = _contextAccessor.HttpContext.Session.GetString(SessionKeyConstants.OrgSelected);

            if (!string.IsNullOrEmpty(obj))
            {
                var tru = JsonSerializer.Deserialize<AcademyTrust>(obj);
                organisation = tru is not null 
                    ? await Kernel.Organisation.CreateFromAcademyTrust(tru, _conditionMapperResolver) 
                    : null;
            }
        }

        if (organisation is null)
        {
            var claim = _contextAccessor.GetOrganisation();
            var resolved = _providerContextResolvers.FirstOrDefault(x => x.ProviderType == Provider.GetType())?.Resolve(claim) ??
                claim;
            organisation = resolved is not null 
                ? Kernel.Organisation.CreateFromClaim(resolved) 
                : null;
        }

        if (organisation is not null)
        {
            _contextAccessor.HttpContext?.Session.SetString(
                SessionKeyConstants.ComputedOrg,
                JsonSerializer.Serialize(organisation));
        }
        
        return organisation;
    }

    private Organisation? Claim
    {
        get
        {
            return field ??= Populate().GetAwaiter().GetResult();
        }
    }

    public Dictionary<string, SchoolType> Type => field ??= BuildAvailableSchoolTypes();
    
    public IProvider Provider
    {
        get
        {
            return _contextAccessor.HttpContext?.Session.GetString(SessionKeyConstants.OrgType) switch
            {
                SessionKeyConstants.OrgTypeAcademy => new PreSixteenProvider(),
                SessionKeyConstants.OrgTypeTrust => new TrustProvider(),
                _ => _providerResolver.Resolve(_contextAccessor.GetOrganisation())
            };
        }
    }

    public IOrganisation Organisation
    {
        [SuppressMessage("Design", "CA1065:Do not raise exceptions in unexpected locations")]
        get
        {
            return _contextAccessor.HttpContext?.Session.GetString(SessionKeyConstants.OrgType) switch
            {
                SessionKeyConstants.OrgTypeAcademy => new EstablishmentOrganisation(Claim!),
                SessionKeyConstants.OrgTypeTrust => TrustOrganisation.CreateFromOrganisation(Claim!),
                _ => _organisationResolver.Resolve(_contextAccessor.GetOrganisation())
            };
        }
    }
    
    public Task<bool> IsValid()
    {
        return Task.FromResult(Claim is not null);
    }

    public async Task<bool> IsAuthorised()
    {
        return await IsValid() && Provider is not NullProvider && await Provider.CanAccess(Claim!);
    }
    
    public static SchoolType DetermineSchoolType(IOrganisation? organisation)
    {
        return organisation?.Category switch
        {
            OrganisationCategory.SingleAcademyTrust => SchoolType.SingleAcademyTrust,
            OrganisationCategory.MultiAcademyTrust => SchoolType.MultiAcademyTrust,
            _ => organisation?.Type switch
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
            },
        };
    }
    
    private Dictionary<string, SchoolType> BuildAvailableSchoolTypes()
    {
        var response = new Dictionary<string, SchoolType>
        {
            { Organisation.Ukrpn, DetermineSchoolType(Organisation) }
        };

        if (Organisation is TrustOrganisation trust)
        {
            foreach (var establishment in trust.Establishments)
            {
                response.Add(establishment.Ukrpn, DetermineSchoolType(establishment));
            }    
        }
        
        return response;
    }
}
