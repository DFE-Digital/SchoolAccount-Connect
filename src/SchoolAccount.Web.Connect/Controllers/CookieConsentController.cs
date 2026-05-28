using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using SchoolAccount.Kernel.Cookie;

namespace SchoolAccount.Web.Connect.Controllers;

[ApiController]
[Route("cookies")]
public class CookieConsentController(IDataProtectionProvider dataProtectionProvider) : ControllerBase
{
    private readonly IDataProtector _protector = dataProtectionProvider.CreateProtector("cookie-consent");

    [HttpPost("consent")]
    public IActionResult SetConsent([FromBody] CookieConsentRequest request)
    {
        if (
            request.Value
            is not CookieConsentConstants.IdValues.Accepted
                and not CookieConsentConstants.IdValues.Rejected
        )
        {
            return BadRequest();
        }

        var protectedValue = _protector.Protect(request.Value);

        Response.Cookies.Append(
            CookieConsentConstants.CookieName,
            protectedValue,
            new CookieOptions
            {
                Path = "/",
                MaxAge = TimeSpan.FromDays(365),
                HttpOnly = true,
                Secure = Request.IsHttps,
                SameSite = SameSiteMode.Lax,
                IsEssential = true,
            }
        );

        return NoContent();
    }
}

public sealed class CookieConsentRequest
{
    public string? Value { get; set; }
}
