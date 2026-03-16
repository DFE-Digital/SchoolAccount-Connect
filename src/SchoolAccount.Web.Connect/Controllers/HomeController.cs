using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Constants;
using SchoolAccount.Application.Features.Tasks.Search.Queries.GetPage;

namespace SchoolAccount.Web.Connect.Controllers;

[Authorize]
public sealed class HomeController(IQueryHandler<TaskSearchQuery, TaskWithSubTasks> handler) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var result = await handler.Handle(new TaskSearchQuery(string.Empty), cancellationToken);

        if (result.IsFailure)
        {
            return Problem(detail: result.Error.Description);
        }

        return View("Index", result.Value);
    }

    [HttpGet("home/task-search")]
    public async Task<ActionResult<TaskWithSubTasks>> TaskSearch(
        [FromQuery] string term,
        CancellationToken cancellationToken
    )
    {
        var result = await handler.Handle(new TaskSearchQuery(term), cancellationToken);

        if (result.IsFailure)
        {
            return Problem(detail: result.Error.Description);
        }

        return Ok(result.Value);
    }

    [HttpGet(RouteConstants.Support)]
    public IActionResult Support()
    {
        return View("Support");
    }

    [HttpGet(RouteConstants.Maintenance)]
    [FeatureGate(FeatureFlagConstants.MaintenanceMode)]
    public IActionResult Maintenance()
    {
        return View();
    }
}
