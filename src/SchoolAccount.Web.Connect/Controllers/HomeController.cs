using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Constants;
using SchoolAccount.Application.Extensions;
using SchoolAccount.Application.Features.CalendarOfItems.Contracts;
using SchoolAccount.Application.Features.CalendarOfItems.Enums;
using SchoolAccount.Application.Features.CalendarOfItems.Query;
using SchoolAccount.Application.Features.Tasks.Search.Queries.GetPage;
using SchoolAccount.Domain.Dtos;
using SchoolAccount.Kernel;
using SchoolAccount.Web.Connect.Builders;
using SchoolAccount.Web.Connect.Extensions;

namespace SchoolAccount.Web.Connect.Controllers;

[Authorize]
public sealed class HomeController(
    IQueryHandler<TaskSearchQuery, TaskWithSubTasksDto> handler,
    IQueryHandler<CalendarOfItemsCustomQuery, CalendarOfItemsPagedResult> customQueryHandler,
    IOrganisationContext organisationContext
) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] int? pageNumber, CancellationToken cancellationToken)
    {
        var date = DateTime.Today;

        var query = new CalendarOfItemsCustomQuery(
            CalendarOfItemsQueryTypes.SubTask,
            new DateOnlyRange(date.StartOfMonth().ToDateOnly(), date.EndOfMonth().ToDateOnly()),
            10,
            pageNumber ?? 1,
            CalendarOfItemsSortMode.NotSpecified,
            $"No required tasks for {date:MMMM yyyy}"
        );

        var result = await customQueryHandler.Handle(query, cancellationToken);

        if (result.IsFailure)
        {
            return Problem(detail: result.Error.Description);
        }

        var currentUri = Request.GetFullRequestUri();
        var dashboardViewBuilder = new DashboardViewBuilder();
        var viewModel = dashboardViewBuilder.Build(result.Value, organisationContext, currentUri);

        return View(viewModel);
    }

    [HttpGet("home/task-search")]
    public async Task<ActionResult<TaskWithSubTasksDto>> TaskSearch(
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
    [AllowAnonymous]
    public IActionResult Maintenance()
    {
        return View();
    }
}
