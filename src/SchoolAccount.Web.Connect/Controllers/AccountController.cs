using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolAccount.Application.Features.Account;

namespace SchoolAccount.Web.Connect.Controllers;

public class AccountController : Controller
{
    [AllowAnonymous]
    [HttpGet(RouteConstants.Account.Login)]
    public IActionResult Login(string? redirect)
    {
        return Challenge(
            new AuthenticationProperties { RedirectUri = redirect ?? "/" },
            OpenIdConnectDefaults.AuthenticationScheme
        );
    }

    [AllowAnonymous]
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

    [AllowAnonymous]
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

    [Authorize]
    [HttpGet("/account/school")]
    public IActionResult Organisation()
    {
        return View(new AccountSchoolViewModel());
    }
}
