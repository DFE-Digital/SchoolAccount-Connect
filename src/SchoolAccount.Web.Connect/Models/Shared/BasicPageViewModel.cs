namespace SchoolAccount.Web.Connect.Models.Shared;

public class BasicPageViewModel(string? organisationName = null)
{
    public string? OrganisationName { get; } = organisationName;

    public bool HasOrganisationName => !string.IsNullOrWhiteSpace(OrganisationName);
}
