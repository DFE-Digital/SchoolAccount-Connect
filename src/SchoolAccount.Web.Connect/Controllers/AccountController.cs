using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using SchoolAccount.Application.Features.Account;

namespace SchoolAccount.Web.Connect.Controllers;

public class AccountController : Controller
{
    public new IActionResult SignOut()
    {
        if (!(User?.Identity?.IsAuthenticated ?? false))
        {
            return RedirectToAction("Index", "Start");
        }
        
        HttpContext.Session.Clear();

        return base.SignOut(
            CookieAuthenticationDefaults.AuthenticationScheme,
            OpenIdConnectDefaults.AuthenticationScheme
        );
    }
    
    public IActionResult SignedOut()
    {
        HttpContext.Session.Clear();

        if (this.User?.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction("Index", "Home");
        }
        
        return RedirectToAction("Index", "Start");
    }

    [HttpGet("/account/school")]
    public IActionResult School()
    {
        return View(new AccountSchoolViewModel());
    }
}