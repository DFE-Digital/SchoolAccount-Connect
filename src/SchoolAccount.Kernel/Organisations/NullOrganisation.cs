using SchoolAccount.Integration.DfESignIn.Models;

namespace SchoolAccount.Kernel.Organisations;

public class NullOrganisation : IOrganisation
{
    public string Ukrpn => string.Empty;
    public string Name => string.Empty;
    public EstablishmentType Establishment => EstablishmentType.Undeclared;
    public OrganisationCategory Category => OrganisationCategory.Undeclared;

    public static IOrganisation Default => new NullOrganisation();
}
