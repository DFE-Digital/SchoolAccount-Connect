using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Constants;
using SchoolAccount.Application.Extensions;
using SchoolAccount.Application.Features.CalendarOfItems.Contracts;
using SchoolAccount.Application.Features.CalendarOfItems.Enums;
using SchoolAccount.Application.Features.CalendarOfItems.Query;
using SchoolAccount.Application.Features.Category.Contracts;
using SchoolAccount.Application.Features.Category.Query;
using SchoolAccount.Application.Features.Tasks.Search.Queries.GetPage;
using SchoolAccount.Domain.Dtos;
using SchoolAccount.Kernel;
using SchoolAccount.Web.Connect.Builders;
using SchoolAccount.Web.Connect.Extensions;

namespace SchoolAccount.Web.Connect.Controllers;

[Authorize]
public sealed class HomeController(
    IQueryHandler<TaskSearchQuery, TaskWithSubTasksDto> handler,
    IQueryHandler<GetAllParentCategoriesQuery, CategoryPagedResult> categoryQueryBuilder,
    IQueryHandler<CalendarOfItemsCustomQuery, CalendarOfItemsPagedResult> customQueryHandler,
    IOrganisationContext organisationContext
) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] int? pageNumber, CancellationToken cancellationToken)
    {
        var date = DateTime.Today;

        var calendarOfItemsQuery = new CalendarOfItemsCustomQuery(
            CalendarOfItemsQueryTypes.SubTask,
            new DateOnlyRange(date.StartOfMonth().ToDateOnly(), date.EndOfMonth().ToDateOnly()),
            10,
            pageNumber ?? 1,
            CalendarOfItemsSortMode.NotSpecified,
            $"No required tasks for {date:MMMM yyyy}"
        );

        var calendarOfItemsResult = await customQueryHandler.Handle(calendarOfItemsQuery, cancellationToken);

        if (calendarOfItemsResult.IsFailure)
        {
            return Problem(detail: calendarOfItemsResult.Error.Description);
        }
        
        var query = new GetAllParentCategoriesQuery();
        var categoryResult = await categoryQueryBuilder.Handle(query, cancellationToken);

        if (categoryResult.IsFailure)
        {
            throw new ApplicationException(categoryResult.Error.Description);
        }

        if (categoryResult.Value.Payload.Count == 0)
        {
            throw new ApplicationException(categoryResult.Error.Description);
        }

        var currentUri = Request.GetFullRequestUri();
        var dashboardViewBuilder = new DashboardViewBuilder();
        var viewModel = dashboardViewBuilder.Build(calendarOfItemsResult.Value, categoryResult.Value, organisationContext, currentUri);

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
