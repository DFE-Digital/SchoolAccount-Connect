using Microsoft.AspNetCore.Mvc;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Features.Tasks.GetById;
using SchoolAccount.Web.Connect.Attributes;
using SchoolAccount.Web.Connect.Extensions;
using SchoolAccount.Web.Connect.Models.Tasks;
using static SchoolAccount.Web.Connect.RouteConstants;

namespace SchoolAccount.Web.Connect.Controllers;

public sealed class TaskController(IQueryHandler<GetTaskByIdQuery, GetTaskByIdResponse> taskHandler) : Controller
{
    [Breadcrumb("Home", Root)]
    [Breadcrumb("Tasks", Category.AllTasks)]
    [HttpGet("Task/{id}")]
    public async Task<ActionResult<GetTaskByIdResponse>> Index(
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

        return View(taskViewModel);
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
