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

    public static string CurrentUserId { get; set; } = DefaultUserId;
    public static OrganisationClaimBuilder? OrganisationClaim { get; set; }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var organisation = (OrganisationClaim ?? OrganisationClaimBuilder.Default).Build();
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, CurrentUserId),
            new Claim(ClaimTypes.Name, CurrentUserId),
            new Claim("organisation", JsonSerializer.Serialize(organisation, JsonSerializerOptions.Web)),
        };

        var identity = new ClaimsIdentity(claims, SchemeName);
        var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), SchemeName);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
