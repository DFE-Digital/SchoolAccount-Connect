using SchoolAccount.Integration.DfESignIn.Models;

namespace SchoolAccount.Kernel;

public interface IOrganisation
{
    public string Ukrpn { get; }
    public string Name { get; }
    
    public EstablishmentType Establishment { get; }
    public OrganisationCategory Category { get; }
}
