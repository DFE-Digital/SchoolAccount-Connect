using SchoolAccount.Integration.DfESignIn;
using SchoolAccount.Integration.DfESignIn.Interfaces;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Resolvers.Interfaces;

public interface IOrganisationResolver
{
    IOrganisation Resolve(OrganisationClaim? claim);
}
