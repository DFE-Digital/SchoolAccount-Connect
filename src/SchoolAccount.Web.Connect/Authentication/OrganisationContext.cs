using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Application.Providers;
using SchoolAccount.Application.Resolvers.Interfaces;
using SchoolAccount.Integration.DfESignIn;
using SchoolAccount.Integration.DfESignIn.Extensions;
using SchoolAccount.Integration.DfESignIn.Interfaces;
using SchoolAccount.Integration.DfESignIn.Providers;
using SchoolAccount.Kernel;

namespace SchoolAccount.Web.Connect.Authentication;

public class OrganisationContext : IOrganisationContext
{
    private readonly IOrganisationResolver _organisationResolver;
    private SchoolType? _schoolType;

    public OrganisationContext(
        IHttpContextAccessor contextAccessor,
        IProviderResolver providerResolver,
        IOrganisationResolver organisationResolver,
        IEnumerable<IProviderContextResolver>  providerContextResolvers
    )
    {
        _organisationResolver = organisationResolver;
        var rawClaim = contextAccessor.GetOrganisation();

        Provider = providerResolver.Resolve(rawClaim);
        Claim = providerContextResolvers
            .FirstOrDefault(x => x.ProviderType == Provider.GetType())?
            .Resolve(rawClaim) ?? rawClaim;
    }

    private OrganisationClaim? Claim { get; }

    public SchoolType Type => _schoolType ??= DetermineSchoolType(Claim);

    public IProvider Provider { get; }

    public IOrganisation Organisation => field ??= _organisationResolver.Resolve(Claim);
    
    public Task<bool> IsValid()
    {
        return Task.FromResult(Claim is not null);
    }

    public async Task<bool> IsAuthorised()
    {
        return await IsValid() && Provider is not NullProvider && await Provider.CanAccess(Claim);
    }
    
    public static SchoolType DetermineSchoolType(OrganisationClaim? claim)
    {
        return claim?.Category?.Id switch
        {
            OrganisationCategory.SingleAcademyTrust => SchoolType.SingleAcademyTrust,
            OrganisationCategory.MultiAcademyTrust => SchoolType.MultiAcademyTrust,
            _ => claim?.Type?.Id switch
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
