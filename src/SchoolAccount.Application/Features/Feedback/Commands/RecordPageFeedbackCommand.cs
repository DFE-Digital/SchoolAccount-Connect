using SchoolAccount.Application.Abstractions.Messaging;

namespace SchoolAccount.Application.Features.Feedback.Commands;

public sealed record RecordPageFeedbackCommand(string PageId, string Value, string Variant, string? Action) : ICommand;
