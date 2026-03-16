namespace SchoolAccount.Integration.DfESignIn.Interfaces;

public interface IProviderResolver
{
    IProvider Resolve(OrganisationClaim? organisation);
}
