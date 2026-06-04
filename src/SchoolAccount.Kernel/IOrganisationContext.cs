using SchoolAccount.Integration.DfESignIn.Interfaces;

namespace SchoolAccount.Kernel;

public interface IOrganisationContext : IProviderContext
{
    public Dictionary<string, SchoolType> Type { get; }
    public IOrganisation Organisation { get; }
    public IOrganisation? Impersonation { get; }
    public IOrganisation Current => Impersonation ?? Organisation;

    public bool IsDsiDetermined { get; }
    public Task<bool> IsValid();
    public Task<bool> IsAuthorised();
}
