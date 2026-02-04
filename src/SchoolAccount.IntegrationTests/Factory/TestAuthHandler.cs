using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace SchoolAccount.IntegrationTests.Factory;

internal sealed class TestAuthHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    internal const string SchemeName = "Test";
    private const string AuthHeader = "X-User";

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // If the test didn't request auth, treat as unauthenticated
        if (!IsAuthenticatedFromHeader())
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }
        
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, AuthHeader),
        };

        var identity = new ClaimsIdentity(claims, SchemeName);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, SchemeName);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
    
    private bool IsAuthenticatedFromHeader()
    {
        if (!Request.Headers.TryGetValue(AuthHeader, out var values))
        {
            return false;
        }
        
        var value = values.FirstOrDefault();
        return !string.IsNullOrWhiteSpace(value);
    }
}