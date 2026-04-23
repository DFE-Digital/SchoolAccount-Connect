namespace SchoolAccount.Web.Connect.Models.Feedback;

public sealed class FeedbackInteractionRequest
{
    public required string EventType { get; init; }
    public required string PageId { get; init; }
    public required string CtaType { get; init; }
    public string? SelectedAnswer { get; init; }
}
