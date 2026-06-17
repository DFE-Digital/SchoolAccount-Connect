using Microsoft.AspNetCore.Mvc;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Features.Tasks.GetAll;
using SchoolAccount.Web.Connect.Attributes;
using static SchoolAccount.Web.Connect.RouteConstants;

namespace SchoolAccount.Web.Connect.Features.Tasks.GetAll;

public sealed class GetAllTasksController(IQueryHandler<GetAllTasksQuery, GetAllTasksResponse> allTasksHandler)
    : Controller
{
    [Breadcrumb("Home", Root)]
    [Breadcrumb("Tasks")]
    [HttpGet(RouteConstants.Task.AllTasks)]
    public async Task<ActionResult<GetAllTasksResponse>> GetAll(
        [FromQuery] GetAllTasksRequest request,
        CancellationToken cancellationToken
    )
    {
        var tasksQuery = new GetAllTasksQuery(request.PageNumber, request.PageSize);
        var tasksResult = await allTasksHandler.Handle(tasksQuery, cancellationToken);

        var tasksViewModel = GetAllTasksViewModelBuilder.Build(tasksResult.Value);

        return View(ViewAddressConstants.Tasks.GetAll, tasksViewModel);
    }
}
