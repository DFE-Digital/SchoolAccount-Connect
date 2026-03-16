using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Abstractions.Telemetry;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Features.Feedback.Commands;

public sealed class RecordPageFeedbackCommandHandler(IFeedbackTelemetryService feedbackTelemetryService)
    : ICommandHandler<RecordPageFeedbackCommand>
{
    public Task<Result> Handle(RecordPageFeedbackCommand command, CancellationToken cancellationToken)
    {
        feedbackTelemetryService.RecordPageFeedback(command);

        return Task.FromResult(Result.Success());
    }
}
