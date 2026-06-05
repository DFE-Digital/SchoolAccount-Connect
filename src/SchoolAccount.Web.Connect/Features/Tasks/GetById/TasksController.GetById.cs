using Microsoft.AspNetCore.Mvc;
using SchoolAccount.Application.Features.Tasks.GetById;
using SchoolAccount.Web.Connect.Attributes;
using SchoolAccount.Web.Connect.Extensions;
using SchoolAccount.Web.Connect.Features.Tasks.GetById;
using static SchoolAccount.Web.Connect.RouteConstants;

// ReSharper disable CheckNamespace

namespace SchoolAccount.Web.Connect.Features.Tasks;

public sealed partial class TasksController
{
    [Breadcrumb("Home", Root)]
    [Breadcrumb("Tasks", RouteConstants.Task.AllTasks)]
    [HttpGet("Task/{id:long}")]
    public async Task<ActionResult<GetTaskByIdResponse>> GetById(
        [FromRoute] long id,
        [FromQuery] TaskViewMode viewMode,
        CancellationToken cancellationToken
    )
    {
        var taskQuery = new GetTaskByIdQuery(id);
        var taskResult = await taskHandler.Handle(taskQuery, cancellationToken);

        if (taskResult.IsFailure)
        {
            return Problem(detail: taskResult.Error.Description);
        }

        AddTopLevelCategoryAsBreadcrumb(taskResult.Value);

        var taskViewModel = new TaskViewModel(taskResult.Value, viewMode);

        return View(ViewAddressConstants.Tasks.GetById, taskViewModel);
    }

    private void AddTopLevelCategoryAsBreadcrumb(GetTaskByIdResponse task)
    {
        // Manage allows multiple top-level categories but for a breadcrumb
        // we can only have one and we have no way to determine which one to use
        // so we just take the first one
        var taskType = task.TaskTypes.First();

        this.AddBreadcrumb(taskType.Name, $"{Category.Index}/{taskType.Id}");
        this.AddBreadcrumb(task.Name);
    }
}
