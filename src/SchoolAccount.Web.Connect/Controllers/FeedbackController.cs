using Microsoft.AspNetCore.Mvc;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Constants;
using SchoolAccount.Application.Features.Feedback.Commands;

namespace SchoolAccount.Web.Connect.Controllers;

[ApiController]
public sealed class FeedbackController(
    ICommandHandler<RecordPageFeedbackCommand> recordPageFeedbackHandler,
    ICommandHandler<RecordFeedbackExitCommand> recordFeedbackExitHandler
) : ControllerBase
{
    private const string FeedbackSubmittedCookieName = "page_feedback_submitted";
    private const string BannerHiddenCookieName = "connect_banner_hidden";

    [HttpPost(RouteConstants.FeedBackRespond)]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Respond(
        [FromForm] string pageId,
        [FromForm] string ctaType,
        [FromForm] string selectedAnswer,
        CancellationToken cancellationToken
    )
    {
        var result = await recordPageFeedbackHandler.Handle(
            new RecordPageFeedbackCommand(AnalyticsEvents.CtaYesNoInteraction, pageId, ctaType, selectedAnswer),
            cancellationToken
        );

        if (result.IsFailure)
        {
            return Problem(detail: result.Error.Description);
        }

        Response.Cookies.Append(
            FeedbackSubmittedCookieName,
            "true",
            new CookieOptions
            {
                HttpOnly = true,
                Secure = Request.IsHttps,
                SameSite = SameSiteMode.Lax,
                Path = "/",
            }
        );

        return Redirect($"{pageId}#page-feedback");
    }

    [HttpGet(RouteConstants.FeedBackCancel)]
    public async Task<IActionResult> Cancel(
        [FromQuery] string pageId,
        [FromQuery] string? ctaType,
        CancellationToken cancellationToken
    )
    {
        var result = await recordPageFeedbackHandler.Handle(
            new RecordPageFeedbackCommand(
                AnalyticsEvents.CtaCancelled,
                pageId,
                ctaType ?? AnalyticsCtaTypes.YesNo,
                null
            ),
            cancellationToken
        );

        if (result.IsFailure)
        {
            return Problem(detail: result.Error.Description);
        }

        Response.Cookies.Delete(FeedbackSubmittedCookieName);

        if (string.Equals(ctaType, AnalyticsCtaTypes.Banner, StringComparison.OrdinalIgnoreCase))
        {
            Response.Cookies.Append(
                BannerHiddenCookieName,
                "true",
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = Request.IsHttps,
                    SameSite = SameSiteMode.Lax,
                    Path = "/",
                }
            );
        }

        return Redirect($"{pageId}#page-feedback");
    }

    [HttpGet(RouteConstants.FeedBackExit)]
    public async Task<IActionResult> RecordFeedbackExit(
        [FromQuery] string pageId,
        [FromQuery] string ctaType,
        CancellationToken cancellationToken
    )
    {
        var result = await recordFeedbackExitHandler.Handle(
            new RecordFeedbackExitCommand(pageId, ctaType),
            cancellationToken
        );

        if (result.IsFailure)
        {
            return Problem(detail: result.Error.Description);
        }

        Response.Cookies.Delete(FeedbackSubmittedCookieName);

        if (string.Equals(ctaType, AnalyticsCtaTypes.Banner, StringComparison.OrdinalIgnoreCase))
        {
            Response.Cookies.Append(
                BannerHiddenCookieName,
                "true",
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = Request.IsHttps,
                    SameSite = SameSiteMode.Lax,
                    Path = "/",
                }
            );
        }

        return Redirect("https://digital-forms.education.gov.uk/smxqhd6u2i");
    }
}
