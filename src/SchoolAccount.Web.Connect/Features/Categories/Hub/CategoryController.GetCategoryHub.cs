using Microsoft.AspNetCore.Mvc;
using SchoolAccount.Application.Features.Categories.GetCategoryHub;
using SchoolAccount.Web.Connect.Attributes;
using SchoolAccount.Web.Connect.Builders;
using SchoolAccount.Web.Connect.Extensions;
using SchoolAccount.Web.Connect.Features.Categories.Hub;
using static SchoolAccount.Web.Connect.RouteConstants;

// ReSharper disable CheckNamespace

namespace SchoolAccount.Web.Connect.Features.Categories;

public sealed partial class CategoryController
{
    private readonly PaginationViewBuilder _paginationViewBuilder = new();

    [Breadcrumb("Home", Root)]
    [Breadcrumb("Tasks", RouteConstants.Task.AllTasks)]
    [HttpGet("categories/{id:int}")]
    public async Task<ActionResult<GetCategoryHubResponse>> GetCategoryHub(
        [FromRoute] int id,
        [FromQuery] CategoryQuery query,
        CancellationToken cancellationToken = default
    )
    {
        var categoryHubQuery = new GetCategoryHubQuery(id, query.PageNumber);
        var categoryHubResult = await categoryHubHandler.Handle(categoryHubQuery, cancellationToken);

        if (categoryHubResult.IsFailure)
        {
            return Problem(detail: categoryHubResult.Error.Description);
        }

        var currentUri = Request.GetFullRequestUri();
        var pagination = _paginationViewBuilder.Build(categoryHubResult.Value.Tasks, currentUri);
        var categoryHubViewModel = new CategoryHubViewModel(categoryHubResult.Value, pagination);

        this.AddBreadcrumb(categoryHubResult.Value.Category.DisplayName);

        return View(ViewAddressConstants.Categories.Hub, categoryHubViewModel);
    }
}
