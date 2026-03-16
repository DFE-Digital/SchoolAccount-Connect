using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SchoolAccount.Web.Connect.Controllers;

public class LoginController : Controller
{
    [AllowAnonymous]
    public IActionResult Login(string? redirect)
    {
        return Challenge(
            new AuthenticationProperties { RedirectUri = redirect ?? "/" },
            OpenIdConnectDefaults.AuthenticationScheme
        );
    }

    [Authorize]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return SignOut(CookieAuthenticationDefaults.AuthenticationScheme, OpenIdConnectDefaults.AuthenticationScheme);
    }
}
