namespace SchoolAccount.Integration.DfESignIn.Interfaces;

public interface IProviderContextResolver
{
    Type ProviderType { get; }
    OrganisationClaim? Resolve(OrganisationClaim? organisationClaim);
}
