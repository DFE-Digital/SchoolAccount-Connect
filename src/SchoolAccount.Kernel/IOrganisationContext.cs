using SchoolAccount.Integration.DfESignIn.Interfaces;

namespace SchoolAccount.Kernel;

public interface IOrganisationContext : IProviderContext
{
    public SchoolType Type { get; }
    public IOrganisation Organisation { get; }

    public Task<bool> IsValid();
    public Task<bool> IsAuthorised();
}
