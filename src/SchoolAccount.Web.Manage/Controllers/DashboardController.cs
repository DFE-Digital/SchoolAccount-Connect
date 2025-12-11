using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SchoolAccount.Web.Manage.Controllers;

[Authorize]
[Route("[controller]/[action]")]
public class DashboardController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}