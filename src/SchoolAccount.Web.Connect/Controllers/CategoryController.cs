using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Features.CalendarOfItems.Contracts;
using SchoolAccount.Application.Features.CalendarOfItems.Query;
using SchoolAccount.Application.Features.CalendarOfItems.Query.Operational;
using SchoolAccount.Application.Features.Category.Contracts;
using SchoolAccount.Application.Features.Category.Enums;
using SchoolAccount.Application.Features.Category.Models;
using SchoolAccount.Application.Features.Category.Query;
using SchoolAccount.Kernel;
using SchoolAccount.Web.Connect.Builders.Categories;
using SchoolAccount.Web.Connect.Extensions;
using SchoolAccount.Web.Connect.Models;

namespace SchoolAccount.Web.Connect.Controllers;

[Authorize]
public class CategoryController(
    IQueryHandler<GetAllParentCategoriesQuery, CategoryPagedResult> categoryQueryBuilder,
    IQueryHandler<GetCategoryByIdQuery, CategoryType> exploreCategoryQueryHandler,
    IQueryHandler<CalendarOfItemsCustomQuery, CalendarOfItemsPagedResult> calendarOfItemsQueryHandler,
    IOrganisationContext organisationContext
) : Controller
{
    [HttpGet(RouteConstants.Category.Index)]
    public async Task<IActionResult> Index(
        [FromQuery] CategoryQuery query,
        CancellationToken cancellationToken = default
    )
    {
        var categoryQuery = new GetAllParentCategoriesQuery(query.PageNumber, query.PageSize);
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
        var viewBuilder = new CategoryListViewBuilder();
        var viewModel = viewBuilder.BuildForPage(result.Value, CategoryListViewModes.Standalone, currentUri);

        return View(viewModel);
    }

    [HttpGet(RouteConstants.Category.Hub)]
    public async Task<IActionResult> Hub(
        [FromRoute] int id,
        [FromQuery] CalendarQuery query,
        CancellationToken cancellationToken = default
    )
    {
        var category = await exploreCategoryQueryHandler.Handle(new GetCategoryByIdQuery(id), cancellationToken);

        if (category.IsFailure)
        {
            throw new ApplicationException(category.Error.Description);
        }

        var results = await calendarOfItemsQueryHandler.Handle(
            new GetSubTasksByCategoriesCalendarOfItemsQuery(
                category.Value.AllCategoryIds,
                query.PageSize,
                query.PageNumber
            ),
            cancellationToken
        );

        if (results.IsFailure)
        {
            throw new ApplicationException(results.Error.Description);
        }

        var currentUri = Request.GetFullRequestUri();
        var viewBuilder = new CategoryHubViewBuilder(organisationContext);
        var viewModel = viewBuilder.Build(results.Value, currentUri, category.Value);

        return View(viewModel);
    }

    [HttpGet(RouteConstants.Category.AllTasks)]
    public async Task<IActionResult> AllTasks(
        [FromQuery] CalendarQuery query,
        CancellationToken cancellationToken = default
    )
    {
        var results = await calendarOfItemsQueryHandler.Handle(
            new GetSubTasksByCategoriesCalendarOfItemsQuery([], query.PageSize, query.PageNumber),
            cancellationToken
        );

        if (results.IsFailure)
        {
            throw new ApplicationException(results.Error.Description);
        }

        var currentUri = Request.GetFullRequestUri();
        var viewBuilder = new CategoryHubViewBuilder(organisationContext);
        var viewModel = viewBuilder.Build(results.Value, currentUri);

        return View(viewModel);
    }
}
