using Microsoft.AspNetCore.Mvc;
using SchoolAccount.Web.Connect.Models;
using SchoolAccount.Web.Connect.Services;

namespace SchoolAccount.Web.Connect.Controllers;

[ApiController]
public class FeedbackController(IFeedbackTelemetryService feedbackTelemetryService) : ControllerBase
{
    [HttpPost(RouteConstants.FeedBack)]
    public IActionResult RecordPageFeedback([FromBody] PageFeedbackRequest request)
    {
        if (!IsValid(request))
        {
            return BadRequest();
        }

        feedbackTelemetryService.RecordPageFeedback(request);

        return NoContent();
    }

    private static bool IsValid(PageFeedbackRequest request) =>
        !string.IsNullOrWhiteSpace(request.PageId) &&
        !string.IsNullOrWhiteSpace(request.Value) &&
        !string.IsNullOrWhiteSpace(request.Variant);
}