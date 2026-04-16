using FluentValidation;
using SchoolAccount.Application.Constants;

namespace SchoolAccount.Application.Features.Feedback.Commands;

public sealed class RecordPageFeedbackCommandValidator : AbstractValidator<RecordPageFeedbackCommand>
{
    public RecordPageFeedbackCommandValidator()
    {
        RuleFor(x => x.PageId).NotEmpty();

        RuleFor(x => x.EventName).NotEmpty().Must(BeValidEvent).WithMessage("Invalid feedback event.");

        RuleFor(x => x.CtaType).NotEmpty().Must(BeValidCtaType).WithMessage("Invalid CTA type.");

        When(
            x => x.EventName == AnalyticsEvents.CtaYesNoInteraction,
            () =>
            {
                RuleFor(x => x.SelectedAnswer)
                    .NotEmpty()
                    .Must(BeValidAnswer)
                    .WithMessage("Selected answer must be 'yes' or 'no'.");
            }
        );

        When(
            x => x.EventName != AnalyticsEvents.CtaYesNoInteraction,
            () =>
            {
                RuleFor(x => x.SelectedAnswer).Null();
            }
        );
    }

    private static bool BeValidEvent(string eventName)
    {
        return eventName
            is AnalyticsEvents.CtaYesNoInteraction
                or AnalyticsEvents.CtaCancelled
                or AnalyticsEvents.CtaDismissed;
    }

    private static bool BeValidCtaType(string ctaType)
    {
        return ctaType is AnalyticsCtaTypes.YesNo or AnalyticsCtaTypes.Banner;
    }

    private static bool BeValidAnswer(string? answer)
    {
        return answer is AnalyticsAnswers.Yes or AnalyticsAnswers.No;
    }
}
