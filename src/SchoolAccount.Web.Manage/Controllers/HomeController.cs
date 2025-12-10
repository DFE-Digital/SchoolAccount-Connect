using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SchoolAccount.Web.Manage.Models;

namespace SchoolAccount.Web.Manage.Controllers;

public sealed class HomeController : Controller
{
    public IActionResult Index()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction("Get", "Dashboard");
        }
        
        return View();
    }
}
