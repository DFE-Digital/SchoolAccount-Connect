using Microsoft.AspNetCore.Mvc;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Features.Tasks.GetById;
using SchoolAccount.Web.Connect.Models.Tasks;

namespace SchoolAccount.Web.Connect.Controllers;

public sealed class TaskController(IQueryHandler<GetTaskByIdQuery, GetTaskByIdResponse> taskHandler) : Controller
{
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

        var taskViewModel = new TaskViewModel(taskResult.Value, viewMode);

        return View(taskViewModel);
    }
}
