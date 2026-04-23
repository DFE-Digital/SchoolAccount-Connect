using SchoolAccount.Application.Abstractions.Messaging;

namespace SchoolAccount.Application.Features.Feedback.Commands;

public sealed record RecordPageFeedbackCommand(string EventName, string PageId, string CtaType, string? SelectedAnswer)
    : ICommand;
