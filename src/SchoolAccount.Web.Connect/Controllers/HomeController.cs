using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SchoolAccount.Web.Connect.Controllers;

public sealed class HomeController : Controller
{
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Index()
    {
        return User.Identity?.IsAuthenticated == true 
            ? View("Index")
            : RedirectToAction("Index", "Login");
    }

    [Authorize]
    [HttpGet("support")]
    public IActionResult Support()
    {
        return View("Support");
    }
    
    
}
