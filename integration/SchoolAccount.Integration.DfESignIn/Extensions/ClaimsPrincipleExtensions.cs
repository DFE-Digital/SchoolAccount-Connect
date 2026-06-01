using System.Security.Claims;
using System.Text.Json;
using SchoolAccount.Integration.DfESignIn;
using SchoolAccount.Integration.DfESignIn.Models;

namespace SchoolAccount.Integration.DfESignIn.Extensions;

public static class ClaimsPrincipleExtensions
{
    public static OrganisationClaim? GetOrganisation(
        this ClaimsPrincipal principal,
        JsonSerializerOptions? options = null
    )
    {
        options ??= new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var organisation = principal.FindFirst(ClaimConstants.Organisation)?.Value;
        return !string.IsNullOrEmpty(organisation)
            ? JsonSerializer.Deserialize<OrganisationClaim>(organisation, options)
            : null;
    }
}
