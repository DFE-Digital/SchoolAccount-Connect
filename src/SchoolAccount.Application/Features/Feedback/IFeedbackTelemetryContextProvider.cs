namespace SchoolAccount.Application.Features.Feedback;

public interface IFeedbackTelemetryContextProvider
{
    Task<FeedbackTelemetryContext> GetContext();
}
