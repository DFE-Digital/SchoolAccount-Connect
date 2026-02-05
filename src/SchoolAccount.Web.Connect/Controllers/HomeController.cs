using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Application.Features.Tasks.Search.Queries.GetPage;

namespace SchoolAccount.Web.Connect.Controllers;

public sealed class HomeController(IPageReadStore pageReadStore) : Controller
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
    
    [AllowAnonymous]
    [HttpGet("home/task-search")]
    public async Task<ActionResult<TaskWithSubTasks>> TaskSearch([FromQuery] string term, CancellationToken cancellationToken)
    {
        var result = await pageReadStore.SearchTasksAsync(new TaskSearchQuery(term), cancellationToken);
        return Ok(result);
    }
}
