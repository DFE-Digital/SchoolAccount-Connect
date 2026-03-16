using FluentValidation;

namespace SchoolAccount.Application.Features.Feedback.Commands;

public sealed class RecordPageFeedbackCommandValidator : AbstractValidator<RecordPageFeedbackCommand>
{
    public RecordPageFeedbackCommandValidator()
    {
        RuleFor(x => x.PageId).NotEmpty();
        RuleFor(x => x.Value).NotEmpty();
        RuleFor(x => x.Variant).NotEmpty();
    }
}
