namespace SchoolAccount.Integration.DfESignIn.Interfaces;

public interface IProvider
{
    int Priority { get; }
    bool IsProvider(OrganisationClaim organisation);
    Task<bool> CanAccess(OrganisationClaim? organisation);
}
