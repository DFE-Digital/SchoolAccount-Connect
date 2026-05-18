using System.Collections.ObjectModel;
using SchoolAccount.Integration.DfESignIn;
using SchoolAccount.Integration.DfESignIn.Interfaces;
using SchoolAccount.Integration.DfESignIn.Models;

namespace SchoolAccount.Kernel;

public interface IOrganisationContext : IProviderContext
{
    public bool IsDsiDetermined { get; }
    public bool IsAuthorised { get; }
    public Dictionary<string, SchoolType> Type { get; }
    public EstablishmentType Establishment { get; }
    public OrganisationCategory Category { get; }
    public IOrganisation Organisation { get; }
}
