using SchoolAccount.Integration.DfESignIn.Models;

namespace SchoolAccount.Integration.DfESignIn.Interfaces;

public interface IProvider
{
    bool IsProvider(OrganisationClaim organisation);
    Task<bool> CanAccess();
}
