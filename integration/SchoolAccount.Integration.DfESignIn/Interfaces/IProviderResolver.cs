using SchoolAccount.Integration.AcademiesApi.Models;
using SchoolAccount.Integration.DfESignIn.Models;

namespace SchoolAccount.Integration.DfESignIn.Interfaces;

public interface IProviderResolver
{
    IProvider Resolve(OrganisationClaim? organisation, AcademyOrganisation? academy, AcademyTrust? trust);
}
