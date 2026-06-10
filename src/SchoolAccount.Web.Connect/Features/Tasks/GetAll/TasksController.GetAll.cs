using Microsoft.AspNetCore.Mvc;
using SchoolAccount.Application.Features.Tasks.GetAll;
using SchoolAccount.Web.Connect.Attributes;
using SchoolAccount.Web.Connect.Extensions;
using SchoolAccount.Web.Connect.Features.Tasks.GetAll;
using static SchoolAccount.Web.Connect.RouteConstants;

// ReSharper disable CheckNamespace

namespace SchoolAccount.Web.Connect.Features.Tasks;

public sealed partial class TasksController
{
    [Breadcrumb("Home", Root)]
    [Breadcrumb("Tasks")]
    [HttpGet(RouteConstants.Task.AllTasks)]
    public async Task<ActionResult<GetAllTasksResponse>> GetAll(
        [FromQuery] TasksQuery query,
        CancellationToken cancellationToken
    )
    {
        var tasksQuery = new GetAllTasksQuery();
        var tasksResult = await allTasksHandler.Handle(tasksQuery, cancellationToken);

        var currentUri = Request.GetFullRequestUri();
        var taskViewModel = new GetAllTasksViewModel(tasksResult.Value);

        return View(ViewAddressConstants.Tasks.GetAll, taskViewModel);
    }
}
