namespace SchoolAccount.Kernel.Organisations;

public class NullOrganisation : IOrganisation
{
    public string Ukrpn => string.Empty;
    public string Name => string.Empty;

    public static IOrganisation Default => new NullOrganisation();
}