using SchoolAccount.Integration.DfESignIn;
using SchoolAccount.Integration.DfESignIn.Interfaces;

namespace SchoolAccount.Kernel;

public interface IOrganisationContext : IProviderContext
{
    public bool IsValid { get; }
    public bool IsAuthorised { get; }
    public SchoolType Type { get; }
    public IOrganisation Organisation { get; }
}
