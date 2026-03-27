using Microsoft.AspNetCore.Mvc;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Features.TaskDetails.ViewModels;
using SchoolAccount.Domain.ViewModels;

namespace SchoolAccount.Web.Connect.Controllers;

public sealed class TaskController(IQueryHandler<TaskDetailQuery, TaskDetailsViewModel> taskHandler) : Controller
{
    [HttpGet("Task")]
    public async Task<ActionResult<TaskDetailsViewModel>> TaskDetailsPage(
        [FromQuery] TaskDetailQuery taskDetailQuery,
        CancellationToken cancellationToken
    )
    {
        var result = await taskHandler.Handle(taskDetailQuery, cancellationToken);

        if (result.IsFailure)
        {
            return Problem(detail: result.Error.Description);
        }

        result.Value.AddRequestDetails(Request);

        return View("TaskDetailsPage", result.Value);
    }
}
