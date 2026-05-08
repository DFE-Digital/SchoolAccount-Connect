using SchoolAccount.Integration.AcademiesApi.Models;
using SchoolAccount.Integration.DfESignIn;
using SchoolAccount.Integration.DfESignIn.Models;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Resolvers.Interfaces;

public interface IOrganisationResolver
{
    IOrganisation Resolve(OrganisationClaim? claim, AcademyOrganisation? academy, AcademyTrust? trust);
}
