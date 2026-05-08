using SchoolAccount.Application.Resolvers.Interfaces;
using SchoolAccount.Integration.AcademiesApi.Models;
using SchoolAccount.Integration.DfESignIn;
using SchoolAccount.Integration.DfESignIn.Models;
using SchoolAccount.Kernel;
using SchoolAccount.Kernel.Organisations;

namespace SchoolAccount.Application.Resolvers;

public class OrganisationResolver : IOrganisationResolver
{
    public IOrganisation Resolve(OrganisationClaim? claim, AcademyOrganisation? academy, AcademyTrust? trust)
    {
        if (trust is not null)
        {
            return new TrustOrganisation(trust);
        }

        if (academy is not null)
        {
            return new EstablishmentOrganisation(academy.Ukprn, academy.EstablishmentName);
        }
        
        return claim?.Category?.Id switch
        {
            OrganisationCategory.SingleAcademyTrust  
                or OrganisationCategory.MultiAcademyTrust 
                => new TrustOrganisation(claim),
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
