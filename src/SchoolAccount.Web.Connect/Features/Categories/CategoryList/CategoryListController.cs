using Microsoft.AspNetCore.Mvc;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Features.Categories.GetParentCategories;
using static SchoolAccount.Web.Connect.RouteConstants;

namespace SchoolAccount.Web.Connect.Features.Categories.CategoryList;

public sealed class CategoryListController(
    IQueryHandler<GetParentCategoriesQuery, GetParentCategoriesResponse> categoryListHandler
) : Controller
{
    [HttpGet(Category.List)]
    public async Task<ActionResult<GetParentCategoriesResponse>> GetParentCategories(
        [FromQuery] CategoryListRequest query,
        CancellationToken cancellationToken = default
    )
    {
        var categoriesQuery = new GetParentCategoriesQuery(query.PageNumber, query.PageSize);
        var categoriesResult = await categoryListHandler.Handle(categoriesQuery, cancellationToken);

        if (categoriesResult.IsFailure)
        {
            return Problem(detail: categoriesResult.Error.Description);
        }

        var categoryListViewModel = CategoryListViewModelBuilder.Build(categoriesResult.Value);

        return View(ViewAddressConstants.Categories.List, categoryListViewModel);
    }
}
