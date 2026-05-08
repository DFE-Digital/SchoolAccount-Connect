using SchoolAccount.Integration.DfESignIn.Models;

namespace SchoolAccount.Web.Connect.Models;

public class SelectAOrganisationViewModel
{
    public string? Message { get; set; }
    public IReadOnlyCollection<OrganisationClaim> Organisations { get; set; } = [];
}