using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SchoolAccount.Tests.Common.Builders;

namespace SchoolAccount.Tests.Common.Fakes;

public class SessionAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder
) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    public const string SchemeName = "SessionAuthenticationTests";
    public const string DefaultUserId = "this-user-is-cool";
    public const string UserIdHeader = "X-Test-User-Id";
    public const string OrganisationHeader = "X-Test-Organisation";

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var userId = Request.Headers[UserIdHeader].FirstOrDefault() ?? DefaultUserId;
        var organisationJson =
            Request.Headers[OrganisationHeader].FirstOrDefault()
            ?? JsonSerializer.Serialize(OrganisationClaimBuilder.Default.Build(), JsonSerializerOptions.Web);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Name, userId),
            new Claim("organisation", organisationJson),
        };

        var identity = new ClaimsIdentity(claims, SchemeName);
        var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), SchemeName);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
