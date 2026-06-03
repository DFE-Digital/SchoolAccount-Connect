using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Constants;
using SchoolAccount.Application.Extensions;
using SchoolAccount.Application.Features.CalendarOfItems.Common.Models;
using SchoolAccount.Application.Features.CalendarOfItems.GetCalendarOfItemsOfNextTenItemsOfSubTasks;
using SchoolAccount.Application.Features.Category.Contracts;
using SchoolAccount.Application.Features.Category.Query;
using SchoolAccount.Application.Features.Shared.Query.Contracts;
using SchoolAccount.Application.Features.Tasks.Search.Queries.GetPage;
using SchoolAccount.Web.Connect.Attributes;
using SchoolAccount.Web.Connect.Builders;
using SchoolAccount.Web.Connect.Builders.Shared;
using SchoolAccount.Web.Connect.Extensions;

namespace SchoolAccount.Web.Connect.Controllers;

public sealed class HomeController(
    IQueryHandler<TaskSearchQuery, TaskSearchResponse> handler,
    IQueryHandler<GetAllParentCategoriesThatHaveAssociatedTasksQuery, CategoryPagedResult> categoryQueryBuilder,
    IQueryHandler<GetCalendarOfItemsOfNextTenItemsOfSubTasksQuery, GenericQueryPagedResult<CalendarOfItemsRow>> calendarOfItemQueryBuilder,
    DashboardViewBuilder dashboardViewBuilder,
    TaskSearchCategoryHubViewBuilder taskSearchCategoryHubViewBuilder,
    BasicPageViewBuilder basicPageViewBuilder
) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] int? pageNumber, CancellationToken cancellationToken)
    {
        var calendarOfItemsQuery = new GetCalendarOfItemsOfNextTenItemsOfSubTasksQuery(DateOnlyExtensions.Today);
        var calendarOfItemsResult = await calendarOfItemQueryBuilder.Handle(calendarOfItemsQuery, cancellationToken);

        if (calendarOfItemsResult.IsFailure)
        {
            return Problem(detail: calendarOfItemsResult.Error.Description);
        }

        var query = new GetAllParentCategoriesThatHaveAssociatedTasksQuery();
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
        var viewModel = dashboardViewBuilder.Build(calendarOfItemsResult.Value, categoryResult.Value, currentUri);

        return View(viewModel);
    }

    [HttpGet("search")]
    public async Task<IActionResult> TaskSearch(
        [FromQuery] string? term,
        [FromQuery] int? pageNumber,
        CancellationToken cancellationToken
    )
    {
        var result = await handler.Handle(new TaskSearchQuery(term, pageNumber ?? 1), cancellationToken);

        if (result.IsFailure)
        {
            return Problem(detail: result.Error.Description);
        }

        var currentUri = Request.GetFullRequestUri();
        var viewModel = taskSearchCategoryHubViewBuilder.Build(result.Value, term, currentUri);

        return View("~/Views/Category/AllTasks.cshtml", viewModel);
    }

    [Breadcrumb("Home", "/")]
    [Breadcrumb("Support", RouteConstants.Support)]
    [HttpGet(RouteConstants.Support)]
    public async Task<IActionResult> Support()
    {
        var model = await basicPageViewBuilder.Build();
        return View("Support", model);
    }

    [Breadcrumb("Home", "/")]
    [Breadcrumb("Cookies", RouteConstants.Cookies)]
    [HttpGet(RouteConstants.Cookies)]
    [AllowAnonymous]
    public async Task<IActionResult> Cookies()
    {
        var model = await basicPageViewBuilder.Build();
        return View("Cookies", model);
    }

    [HttpGet(RouteConstants.Maintenance)]
    [FeatureGate(FeatureFlagConstants.MaintenanceMode)]
    [AllowAnonymous]
    public async Task<IActionResult> Maintenance()
    {
        var model = await basicPageViewBuilder.Build();
        return View("Maintenance", model);
    }
}
