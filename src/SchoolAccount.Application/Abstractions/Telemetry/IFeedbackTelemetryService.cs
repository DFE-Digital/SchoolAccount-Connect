using SchoolAccount.Application.Features.Feedback.Commands;

namespace SchoolAccount.Application.Abstractions.Telemetry;

public interface IFeedbackTelemetryService
{
    void RecordPageFeedback(RecordPageFeedbackCommand command);
}