using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SchoolAccount.Tests.Common.Builders;

namespace SchoolAccount.IntegrationTests.Features.Authentication.Handlers;


public class SessionAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    public const string SchemeName = "SessionAuthenticationTests";
    public static string CurrentUserId { get; set; } = "this-user-is-cool";
    public static OrganisationClaimBuilder? OrganisationClaim { get; set; }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var organisation = (OrganisationClaim ?? OrganisationClaimBuilder.Default).Build();
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, CurrentUserId),
            new Claim(ClaimTypes.Name, CurrentUserId),
            new Claim("organisation", JsonConvert.SerializeObject(organisation)),
        };

        var identity = new ClaimsIdentity(claims, SchemeName);
        var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), SchemeName);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}