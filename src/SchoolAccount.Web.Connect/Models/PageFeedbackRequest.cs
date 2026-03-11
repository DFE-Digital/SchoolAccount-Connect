namespace SchoolAccount.Web.Connect.Models;

public record PageFeedbackRequest(
    string PageId,
    string Value,
    string Variant,
    string? Action
);
