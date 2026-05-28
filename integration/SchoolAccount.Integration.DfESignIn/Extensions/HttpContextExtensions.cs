using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace SchoolAccount.Integration.DfESignIn.Extensions;

public static class HttpContextExtensions
{
    public static OrganisationClaim? GetOrganisation(
        this IHttpContextAccessor context,
        JsonSerializerOptions? options = null
    )
    {
        return context.HttpContext.GetOrganisation(options);
    }

    public static OrganisationClaim? GetOrganisation(this HttpContext? context, JsonSerializerOptions? options = null)
    {
        return context?.User.GetOrganisation(options);
    }
}
