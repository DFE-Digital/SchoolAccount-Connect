using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SchoolAccount.Web.Connect.Features.Account;

[AllowAnonymous]
public class AccountController : Controller
{
    [HttpGet(RouteConstants.Account.Login)]
    public IActionResult Login(string? redirect)
    {
        return Challenge(
            new AuthenticationProperties { RedirectUri = redirect ?? "/" },
            OpenIdConnectDefaults.AuthenticationScheme
        );
    }

    [HttpGet(RouteConstants.Account.SignOut)]
    public new async Task<IActionResult> SignOut()
    {
        if (!(User?.Identity?.IsAuthenticated ?? false))
        {
            return RedirectToAction("Index", "Start");
        }

        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        HttpContext.Session.Clear();

        return base.SignOut(
            CookieAuthenticationDefaults.AuthenticationScheme,
            OpenIdConnectDefaults.AuthenticationScheme
        );
    }

    [HttpGet(RouteConstants.Account.SignedOut)]
    public IActionResult SignedOut()
    {
        HttpContext.Session.Clear();

        if (this.User?.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction("Index", "Home");
        }

        return RedirectToAction("Index", "Start");
    }
}
