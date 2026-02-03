using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SchoolAccount.Web.Connect.Controllers;

[AllowAnonymous]
public sealed class HomeController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return User.Identity?.IsAuthenticated == true 
            ? RedirectToAction("Get", "Dashboard") 
            : RedirectToAction("Index", "Login");
    }

    [Authorize]
    [HttpGet("support")]
    public IActionResult Support()
    {
        return View("Support");
    }
}
