using Microsoft.AspNetCore.Mvc;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Features.CalendarOfItems.Models;
using SchoolAccount.Application.Features.CalendarOfItems.Query;
using SchoolAccount.Application.Features.CalendarOfItems.Query.Operational;
using SchoolAccount.Application.Features.Category.Contracts;
using SchoolAccount.Application.Features.Category.Enums;
using SchoolAccount.Application.Features.Category.Models;
using SchoolAccount.Application.Features.Category.Query;
using SchoolAccount.Application.Features.Shared.Query.Contracts;
using SchoolAccount.Web.Connect.Attributes;
using SchoolAccount.Web.Connect.Builders.Categories;
using SchoolAccount.Web.Connect.Extensions;
using SchoolAccount.Web.Connect.Models;
using static SchoolAccount.Web.Connect.RouteConstants;

namespace SchoolAccount.Web.Connect.Controllers;

public class CategoryController(
    IQueryHandler<GetAllParentCategoriesThatHaveAssociatedTasksQuery, CategoryPagedResult> categoryQueryBuilder,
    IQueryHandler<GetCategoryByIdQuery, CategoryType> exploreCategoryQueryHandler,
    IQueryHandler<CalendarOfItemsCustomQuery, QueryPagedResult<CalendarOfItemsRow>> calendarOfItemsQueryHandler,
    CategoryHubViewBuilder categoryHubViewBuilder,
    CategoryListViewBuilder categoryListViewBuilder
) : Controller
{
    [HttpGet(Category.Index)]
    public async Task<IActionResult> Index(
        [FromQuery] CategoryQuery query,
        CancellationToken cancellationToken = default
    )
    {
        var categoryQuery = new GetAllParentCategoriesThatHaveAssociatedTasksQuery(query.PageNumber, query.PageSize);
        var result = await categoryQueryBuilder.Handle(categoryQuery, cancellationToken);

        if (result.IsFailure)
        {
            throw new ApplicationException(result.Error.Description);
        }

        if (result.Value.Payload.Count == 0)
        {
            throw new ApplicationException(result.Error.Description);
        }

        var currentUri = Request.GetFullRequestUri();
        var viewModel = categoryListViewBuilder.BuildForPage(
            result.Value,
            CategoryListViewModes.Standalone,
            currentUri
        );

        return View(viewModel);
    }

    [Breadcrumb("Home", Root)]
    [Breadcrumb("Tasks", Category.AllTasks)]
    [HttpGet(Category.Hub)]
    public async Task<IActionResult> Hub(
        [FromRoute] int id,
        [FromQuery] CalendarQuery query,
        CancellationToken cancellationToken = default
    )
    {
        var category = await exploreCategoryQueryHandler.Handle(new GetCategoryByIdQuery(id), cancellationToken);

        if (category.IsFailure)
        {
            return NotFound();
        }

        var results = await calendarOfItemsQueryHandler.Handle(
            new GetTasksByCategoriesCalendarOfItemsQuery(
                category.Value.AllCategoryIds,
                query.PageSize,
                query.PageNumber
            ),
            cancellationToken
        );

        if (results.IsFailure)
        {
            return NotFound();
        }

        var currentUri = Request.GetFullRequestUri();
        var viewModel = categoryHubViewBuilder.Build(results.Value, currentUri, category.Value);

        this.AddBreadcrumb(category.Value.DisplayName);

        return View(viewModel);
    }

    [Breadcrumb("Home", Root)]
    [Breadcrumb("Tasks")]
    [HttpGet(Category.AllTasks)]
    public async Task<IActionResult> AllTasks(
        [FromQuery] CalendarQuery query,
        CancellationToken cancellationToken = default
    )
    {
        var results = await calendarOfItemsQueryHandler.Handle(
            new GetTasksByCategoriesCalendarOfItemsQuery([], query.PageSize, query.PageNumber),
            cancellationToken
        );

        if (results.IsFailure)
        {
            return NotFound();
        }

        var currentUri = Request.GetFullRequestUri();
        var viewModel = categoryHubViewBuilder.Build(results.Value, currentUri);

        return View(viewModel);
    }
}
