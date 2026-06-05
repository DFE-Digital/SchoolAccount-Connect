using Microsoft.AspNetCore.Mvc;
using SchoolAccount.Application.Features.Tasks.GetAll;
using SchoolAccount.Web.Connect.Attributes;
using SchoolAccount.Web.Connect.Builders;
using SchoolAccount.Web.Connect.Extensions;
using SchoolAccount.Web.Connect.Features.Tasks.GetAll;
using static SchoolAccount.Web.Connect.RouteConstants;

// ReSharper disable CheckNamespace

namespace SchoolAccount.Web.Connect.Features.Tasks;

public sealed partial class TasksController
{
    private readonly PaginationViewBuilder _paginationViewBuilder = new();

    [Breadcrumb("Home", Root)]
    [Breadcrumb("Tasks")]
    [HttpGet(RouteConstants.Task.AllTasks)]
    public async Task<ActionResult<GetAllTasksResponse>> GetAll(
        [FromQuery] TasksQuery query,
        CancellationToken cancellationToken
    )
    {
        var tasksQuery = new GetAllTasksQuery(query.PageNumber, query.PageSize);
        var tasksResult = await allTasksHandler.Handle(tasksQuery, cancellationToken);

        var currentUri = Request.GetFullRequestUri();
        var pagination = _paginationViewBuilder.Build(tasksResult.Value.Tasks, currentUri);
        var taskViewModel = new GetAllTasksViewModel(tasksResult.Value, pagination);

        return View(ViewAddressConstants.Tasks.GetAll, taskViewModel);
    }
}
