using System.Security.Claims;
using System.Text.Json;
using SchoolAccount.Integration.DfESignIn;

namespace SchoolAccount.Web.Connect.Extensions;

public static class ClaimsPrincipleExtensions
{
    public static OrganisationClaim? GetOrganisation(this ClaimsPrincipal principal,
        JsonSerializerOptions? options = null)
    {
        options ??= new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var organisation = principal.FindFirst(ClaimConstants.Organisation)?.Value;
        return !string.IsNullOrEmpty(organisation)
            ? JsonSerializer.Deserialize<OrganisationClaim>(organisation, options)
            : null;
    }
}