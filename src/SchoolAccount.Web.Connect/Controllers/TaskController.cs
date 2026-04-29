using Microsoft.AspNetCore.Mvc;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Features.Tasks.GetById;

namespace SchoolAccount.Web.Connect.Controllers;

public sealed class TaskController(IQueryHandler<GetTaskByIdQuery, TaskResponse> taskHandler) : Controller
{
    [HttpGet("Task/{id}")]
    public async Task<ActionResult<TaskResponse>> Index(
        GetTaskByIdQuery taskDetailQuery,
        CancellationToken cancellationToken
    )
    {
        var result = await taskHandler.Handle(taskDetailQuery, cancellationToken);

        if (result.IsFailure)
        {
            return Problem(detail: result.Error.Description);
        }

        return View(result.Value);
    }
}
