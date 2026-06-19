using Microsoft.AspNetCore.Mvc;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Features.Categories.GetCategoryHub;
using SchoolAccount.Web.Connect.Attributes;
using SchoolAccount.Web.Connect.Extensions;
using static SchoolAccount.Web.Connect.RouteConstants;

namespace SchoolAccount.Web.Connect.Features.Categories.CategoryHub;

public sealed class CategoryHubController(IQueryHandler<GetCategoryHubQuery, GetCategoryHubResponse> categoryHubHandler)
    : Controller
{
    [Breadcrumb("Home", Root)]
    [Breadcrumb("Tasks", RouteConstants.Task.AllTasks)]
    [HttpGet(Category.Hub)]
    public async Task<ActionResult<GetCategoryHubResponse>> GetCategoryHub(
        [FromRoute] int id,
        [FromQuery] CategoryHubRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var categoryHubQuery = new GetCategoryHubQuery(id, request.PageNumber, request.PageSize);
        var categoryHubResult = await categoryHubHandler.Handle(categoryHubQuery, cancellationToken);

        if (categoryHubResult.IsFailure)
        {
            return Problem(detail: categoryHubResult.Error.Description);
        }

        var categoryHubViewModel = CategoryHubViewModelBuilder.Build(categoryHubResult.Value);

        this.AddBreadcrumb(categoryHubResult.Value.DisplayName);

        return View(ViewAddressConstants.Categories.Hub, categoryHubViewModel);
    }
}
