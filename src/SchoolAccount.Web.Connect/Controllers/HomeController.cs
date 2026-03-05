using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Features.Tasks.Search.Queries.GetPage;
using SchoolAccount.Integration.DfESignIn.Attributes;
using SchoolAccount.Integration.DfESignIn.Providers;
using SchoolAccount.Kernel;
using SchoolAccount.Web.Connect.Authentication.Attributes;

namespace SchoolAccount.Web.Connect.Controllers;

[Authorize]
public sealed class HomeController(
    IQueryHandler<TaskSearchQuery, TaskWithSubTasks> handler
) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var result = await handler.Handle(
            new TaskSearchQuery(string.Empty),
            cancellationToken);

        if (result.IsFailure)
        {
            return Problem(detail: result.Error.Description);
        }

        return View("Index", result.Value);
    }

    [HttpGet("support")]
    public IActionResult Support()
    {
        return View("Support");
    }

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