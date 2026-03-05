using SchoolAccount.Integration.DfESignIn;
using SchoolAccount.Integration.DfESignIn.Interfaces;

namespace SchoolAccount.Kernel;

public interface IOrganisationContext : IProviderContext
{
    public bool IsValid { get; }
    public bool IsAuthenticated { get; }
    public string Ukrpn { get; }
    public string Name { get; }
    public SchoolType Type { get; }
    public EstablishmentType Establishment { get; }
    public OrganisationCategory Category { get; }
}