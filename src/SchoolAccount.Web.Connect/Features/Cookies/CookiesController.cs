using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using SchoolAccount.Kernel.Cookie;
using SchoolAccount.Web.Connect.Attributes;
using static SchoolAccount.Web.Connect.RouteConstants;

namespace SchoolAccount.Web.Connect.Features.Cookies;

[AllowAnonymous]
public sealed class CookiesController(IDataProtectionProvider dataProtectionProvider) : Controller
{
    private readonly IDataProtector _protector = dataProtectionProvider.CreateProtector("cookie-consent");

    [Breadcrumb("Home", Root)]
    [Breadcrumb("Cookies")]
    [HttpGet(RouteConstants.Cookies)]
    public IActionResult Cookies()
    {
        return View();
    }

    [HttpPost("cookies/consent")]
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
