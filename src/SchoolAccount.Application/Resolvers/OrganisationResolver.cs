using SchoolAccount.Application.Resolvers.Interfaces;
using SchoolAccount.Integration.AcademiesApi.Models;
using SchoolAccount.Integration.DfESignIn;
using SchoolAccount.Integration.DfESignIn.Models;
using SchoolAccount.Kernel;
using SchoolAccount.Kernel.Organisations;

namespace SchoolAccount.Application.Resolvers;

public class OrganisationResolver : IOrganisationResolver
{
    public IOrganisation Resolve(OrganisationClaim? claim)
    {
        return claim?.Category?.Id switch
        {
            OrganisationCategory.SingleAcademyTrust  
                or OrganisationCategory.MultiAcademyTrust 
                => TrustOrganisation.CreateFromClaim(claim),
            OrganisationCategory.LocalAuthority 
                => new LocalAuthorityOrganisation(claim),
            OrganisationCategory.FurtherEducation 
                => new FurtherEducationOrganisation(claim),
            OrganisationCategory.Establishment 
                => new EstablishmentOrganisation(claim),
            _ => NullOrganisation.Default,
        };
    }
}
