using Microsoft.AspNetCore.Mvc;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Features.Feedback.Commands;

namespace SchoolAccount.Web.Connect.Controllers;

[ApiController]
public sealed class FeedbackController(
    ICommandHandler<RecordPageFeedbackCommand> recordPageFeedbackHandler,
    ICommandHandler<RecordFeedbackExitCommand> recordFeedbackExitHandler
) : ControllerBase
{
    [HttpPost(RouteConstants.FeedBack)]
    public async Task<IActionResult> RecordPageFeedback(
        [FromBody] RecordPageFeedbackCommand command,
        CancellationToken cancellationToken
    )
    {
        var result = await recordPageFeedbackHandler.Handle(command, cancellationToken);

        if (result.IsFailure)
        {
            return Problem(detail: result.Error.Description);
        }

        return NoContent();
    }

    [HttpGet(RouteConstants.FeedBackExit)]
    public async Task<IActionResult> RecordFeedbackExit(
        [FromQuery] string pageId,
        [FromQuery] string ctaType,
        [FromQuery] string returnUrl,
        CancellationToken cancellationToken
    )
    {
        if (!IsAllowedFeedbackUrl(returnUrl))
        {
            return BadRequest("Invalid returnUrl.");
        }

        var result = await recordFeedbackExitHandler.Handle(
            new RecordFeedbackExitCommand(pageId, ctaType),
            cancellationToken
        );

        if (result.IsFailure)
        {
            return Problem(detail: result.Error.Description);
        }

        return Redirect(returnUrl);
    }

    private static bool IsAllowedFeedbackUrl(string returnUrl)
    {
        if (!Uri.TryCreate(returnUrl, UriKind.Absolute, out var uri))
        {
            return false;
        }

        return uri.Scheme == Uri.UriSchemeHttps
            && string.Equals(uri.Host, "digital-forms.education.gov.uk", StringComparison.OrdinalIgnoreCase);
    }
}
