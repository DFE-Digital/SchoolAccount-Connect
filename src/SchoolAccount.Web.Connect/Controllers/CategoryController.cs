using Microsoft.AspNetCore.Mvc;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Features.Categories.Contracts;
using SchoolAccount.Application.Features.Categories.Enums;
using SchoolAccount.Application.Features.Categories.Query;
using SchoolAccount.Web.Connect.Builders.Categories;
using SchoolAccount.Web.Connect.Extensions;
using SchoolAccount.Web.Connect.Features.Categories;
using static SchoolAccount.Web.Connect.RouteConstants;

namespace SchoolAccount.Web.Connect.Controllers;

public class CategoryController(
    IQueryHandler<GetAllParentCategoriesThatHaveAssociatedTasksQuery, CategoryPagedResult> categoryQueryBuilder,
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
}
