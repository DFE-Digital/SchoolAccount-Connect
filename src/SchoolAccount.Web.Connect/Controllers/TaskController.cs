using Microsoft.AspNetCore.Mvc;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Features.Tasks.GetById;

namespace SchoolAccount.Web.Connect.Controllers;

public sealed class TaskController(IQueryHandler<GetTaskByIdQuery, GetTaskByIdResponse> taskHandler) : Controller
{
    [HttpGet("Task/{id}")]
    public async Task<ActionResult<GetTaskByIdResponse>> Index(
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
