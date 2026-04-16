using SchoolAccount.Application.Abstractions.Messaging;

namespace SchoolAccount.Application.Features.Feedback.Commands;

public sealed record RecordFeedbackExitCommand(string PageId, string CtaType) : ICommand;
