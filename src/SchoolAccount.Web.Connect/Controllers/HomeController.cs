using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Features.Tasks.Search.Queries.GetPage;

namespace SchoolAccount.Web.Connect.Controllers;

public sealed class HomeController(IQueryHandler<TaskSearchQuery, TaskWithSubTasks> handler) : Controller
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        if (User.Identity?.IsAuthenticated != true)
        {
            return RedirectToAction("Index", "Login");
        }

        var result = await handler.Handle(
            new TaskSearchQuery(string.Empty),
            cancellationToken);

        if (result.IsFailure)
        {
            return Problem(detail: result.Error.Description);
        }

        return View("Index", result.Value);
    }

    [Authorize]
    [HttpGet("support")]
    public IActionResult Support()
    {
        return View("Support");
    }

    [AllowAnonymous]
    [HttpGet("home/task-search")]
    public async Task<ActionResult<TaskWithSubTasks>> TaskSearch(
        [FromQuery] string term,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(
            new TaskSearchQuery(term),
            cancellationToken);

        if (result.IsFailure)
        {
            return Problem(detail: result.Error.Description);
        }

        return Ok(result.Value);
    }
}