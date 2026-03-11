using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using SchoolAccount.Web.Connect.Models;

namespace SchoolAccount.Web.Connect.Controllers;

[ApiController]
public class FeedbackController(ILogger<FeedbackController> logger) : ControllerBase
{
    [HttpPost("/feedback/page-useful")]
    public IActionResult PageUseful([FromBody] PageFeedbackRequest request)
    {
        var userIdentifier = User?.Identity?.Name;

        var hashedUserId = HashUserId(userIdentifier);

        logger.LogInformation(
            "Page feedback recorded. EventName: {EventName}, Variant: {Variant}, Value: {Value}, PageId: {PageId}, UserId: {UserId}",
            "page_feedback_response",
            request.Variant,
            request.Value,
            request.PageId,
            hashedUserId
        );

        return Ok();
    }

    private static string HashUserId(string? userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            return "anonymous";
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(userId));

        return Convert.ToHexString(bytes);
    }
}