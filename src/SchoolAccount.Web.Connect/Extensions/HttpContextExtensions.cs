using System.Text.Json;
using SchoolAccount.Integration.DfESignIn;

namespace SchoolAccount.Web.Connect.Extensions;

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

    public static string GetCurrentEndpoint(this HttpContext context)
    {
        var endpoint = context.GetEndpoint();
        return (endpoint as RouteEndpoint)?.RoutePattern?.RawText ?? endpoint?.DisplayName ?? string.Empty;
    }

    public static Uri GetFullRequestUri(this IHttpContextAccessor context)
    {
        return context.HttpContext!.GetFullRequestUri();
    }

    public static Uri GetFullRequestUri(this HttpContext context)
    {
        return context.Request.GetFullRequestUri();
    }
}
