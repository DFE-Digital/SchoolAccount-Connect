using System.Security.Claims;

namespace SchoolAccount.Infrastructure.Authentication;

internal static class ClaimsPrincipalExtensions
{
    public static string GetUserId(this ClaimsPrincipal? principal)
    {
        var userId = principal?.FindFirstValue(ClaimTypes.NameIdentifier);

        return userId ?? throw new ApplicationException("User id is unavailable");
    }
}
