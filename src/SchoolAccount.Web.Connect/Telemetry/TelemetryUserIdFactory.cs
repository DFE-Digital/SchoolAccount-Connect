using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SchoolAccount.Web.Connect.Telemetry;

[SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase")]
public static class TelemetryUserIdFactory
{
    public static string CreateHashedUserId(ClaimsPrincipal user)
    {
        ArgumentNullException.ThrowIfNull(user);

        var rawUserId =
            user.FindFirst("sub")?.Value
            ?? user.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? throw new InvalidOperationException("No stable user identifier claim was found.");

        var bytes = Encoding.UTF8.GetBytes(rawUserId);
        var hash = SHA256.HashData(bytes);

        return Convert.ToHexString(hash).ToLowerInvariant();
    }
}
