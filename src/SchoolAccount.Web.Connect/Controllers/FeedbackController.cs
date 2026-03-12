using Microsoft.AspNetCore.Mvc;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Features.Feedback.Commands;

namespace SchoolAccount.Web.Connect.Controllers;

[ApiController]
public sealed class FeedbackController(
    ICommandHandler<RecordPageFeedbackCommand> handler
) : ControllerBase
{
    [HttpPost(RouteConstants.FeedBack)]
    public async Task<IActionResult> RecordPageFeedback(
        [FromBody] RecordPageFeedbackCommand command,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
        {
            return Problem(detail: result.Error.Description);
        }

        return NoContent();
    }
}