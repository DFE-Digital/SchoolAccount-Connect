using Microsoft.AspNetCore.Mvc;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Features.Tasks.Search;

namespace SchoolAccount.Web.Connect.Features.Tasks.Search;

public sealed class SearchTasksController(IQueryHandler<SearchTasksQuery, SearchTasksResponse> taskSearchHandler)
    : Controller
{
    [HttpGet(RouteConstants.Search)]
    public async Task<IActionResult> SearchTasks(SearchTasksRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var searchTasksQuery = new SearchTasksQuery(request.Term, request.PageNumber, request.PageSize);
        var searchTasksResult = await taskSearchHandler.Handle(searchTasksQuery, cancellationToken);

        if (searchTasksResult.IsFailure)
        {
            return Problem(detail: searchTasksResult.Error.Description);
        }

        var searchTasksViewModel = SearchTasksViewModelBuilder.Build(searchTasksResult.Value, request.Term);

        return View(searchTasksViewModel);
    }
}
