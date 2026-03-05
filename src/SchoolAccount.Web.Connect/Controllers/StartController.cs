using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SchoolAccount.Web.Connect.Controllers;

public class StartController : Controller
{
    [AllowAnonymous]
    [HttpGet("/start")]
    public IActionResult Index()
    {
        return View(); 
    }
}