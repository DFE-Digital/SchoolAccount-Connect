using FluentValidation.TestHelper;
using SchoolAccount.Application.Constants;
using SchoolAccount.Application.Features.Feedback.Commands;
using Xunit;

namespace SchoolAccount.Application.UnitTests.Features.Feedback.Commands;

public class RecordPageFeedbackCommandValidatorTests
{
    private readonly RecordPageFeedbackCommandValidator _validator;

    public RecordPageFeedbackCommandValidatorTests()
    {
        _validator = new RecordPageFeedbackCommandValidator();
    }

    [Fact]
    public void Validator_Has_Validation_Error_When_PageId_Is_Empty()
    {
        var command = new RecordPageFeedbackCommand(
            AnalyticsEvents.CtaYesNoInteraction,
            "",
            AnalyticsCtaTypes.YesNo,
            AnalyticsAnswers.Yes
        );

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.PageId);
    }

    [Fact]
    public void Validator_Has_Validation_Error_When_EventName_Is_Empty()
    {
        var command = new RecordPageFeedbackCommand("", "1", AnalyticsCtaTypes.YesNo, AnalyticsAnswers.Yes);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.EventName);
    }

    [Theory]
    [InlineData(AnalyticsEvents.LoginSucceeded)]
    [InlineData(AnalyticsEvents.LoginFailed)]
    [InlineData(AnalyticsEvents.PageVisited)]
    [InlineData(AnalyticsEvents.BannerExposureAssigned)]
    [InlineData(AnalyticsEvents.CtaFeedbackExit)]
    [InlineData("invalid")]
    public void Validator_Has_Validation_Error_When_EventName_Is_Invalid(string eventName)
    {
        var command = new RecordPageFeedbackCommand(eventName, "1", AnalyticsCtaTypes.YesNo, AnalyticsAnswers.Yes);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.EventName).WithErrorMessage("Invalid feedback event.");
    }

    [Fact]
    public void Validator_Has_Validation_Error_When_CtaType_Is_Empty()
    {
        var command = new RecordPageFeedbackCommand(AnalyticsEvents.CtaYesNoInteraction, "1", "", AnalyticsAnswers.Yes);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.CtaType);
    }

    [Fact]
    public void Validator_Has_Validation_Error_When_CtaType_Is_Invalid()
    {
        var command = new RecordPageFeedbackCommand(
            AnalyticsEvents.CtaYesNoInteraction,
            "1",
            "invalid",
            AnalyticsAnswers.Yes
        );

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.CtaType).WithErrorMessage("Invalid CTA type.");
    }

    [Fact]
    public void Validator_Has_Validation_Error_When_Answer_Is_Empty_And_EventName_Is_CtaYesNoInteraction()
    {
        var command = new RecordPageFeedbackCommand(
            AnalyticsEvents.CtaYesNoInteraction,
            "1",
            AnalyticsCtaTypes.YesNo,
            ""
        );

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.SelectedAnswer);
    }

    [Fact]
    public void Validator_Has_Validation_Error_When_Answer_Is_Invalid_And_EventName_Is_CtaYesNoInteraction()
    {
        var command = new RecordPageFeedbackCommand(
            AnalyticsEvents.CtaYesNoInteraction,
            "1",
            AnalyticsCtaTypes.YesNo,
            "invalid"
        );

        var result = _validator.TestValidate(command);

        result
            .ShouldHaveValidationErrorFor(x => x.SelectedAnswer)
            .WithErrorMessage("Selected answer must be 'yes' or 'no'.");
    }

    [Fact]
    public void Validator_Has_Validation_Error_When_Answer_Is_Not_Null_And_EventName_Is_Not_CtaYesNoInteraction()
    {
        var command = new RecordPageFeedbackCommand(
            AnalyticsEvents.PageVisited,
            "1",
            AnalyticsCtaTypes.Banner,
            AnalyticsAnswers.Yes
        );

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.SelectedAnswer);
    }

    [Theory]
    [InlineData(AnalyticsEvents.CtaYesNoInteraction, "1", AnalyticsCtaTypes.YesNo, AnalyticsAnswers.Yes)]
    [InlineData(AnalyticsEvents.CtaDismissed, "1", AnalyticsCtaTypes.Banner, null)]
    public void Validator_Passes_When_All_Fields_Are_Valid(
        string eventName,
        string pageId,
        string ctaType,
        string? selectedAnswer
    )
    {
        var command = new RecordPageFeedbackCommand(eventName, pageId, ctaType, selectedAnswer);

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }
}
