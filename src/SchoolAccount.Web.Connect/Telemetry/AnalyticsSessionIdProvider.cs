using System.Security.Claims;
using SchoolAccount.Application.Constants;

namespace SchoolAccount.Web.Connect.Telemetry;

public static class AnalyticsSessionIdProvider
{
    public static string EnsureSessionIdClaim(ClaimsPrincipal principal)
    {
        ArgumentNullException.ThrowIfNull(principal);

        var existingSessionId = principal.FindFirst(AnalyticsClaimTypes.SessionId)?.Value;
        if (!string.IsNullOrWhiteSpace(existingSessionId))
        {
            return existingSessionId;
        }

        if (principal.Identity is not ClaimsIdentity identity)
        {
            throw new InvalidOperationException("The authenticated principal does not contain a claims identity.");
        }

        var sessionId = Guid.NewGuid().ToString("N");
        identity.AddClaim(new Claim(AnalyticsClaimTypes.SessionId, sessionId));

        return sessionId;
    }

    public static string? GetSessionId(ClaimsPrincipal principal)
    {
        ArgumentNullException.ThrowIfNull(principal);

        return principal.FindFirst(AnalyticsClaimTypes.SessionId)?.Value;
    }
}
