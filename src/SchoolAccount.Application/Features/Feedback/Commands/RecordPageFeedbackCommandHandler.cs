using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Constants;
using SchoolAccount.Application.Features.Telemetry.Commands;
using SchoolAccount.Application.Features.Telemetry.Enums;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Features.Feedback.Commands;

public sealed class RecordPageFeedbackCommandHandler(
    ICommandHandler<TrackAnalyticsTelemetryCommand> telemetryCommandHandler,
    IFeedbackTelemetryContextProvider feedbackTelemetryContextProvider
) : ICommandHandler<RecordPageFeedbackCommand>
{
    public async Task<Result> Handle(RecordPageFeedbackCommand command, CancellationToken cancellationToken)
    {
        var context = await feedbackTelemetryContextProvider.GetContext();

        var tags = BuildTags(command.PageId, command.CtaType, command.SelectedAnswer, context);

        var result = await telemetryCommandHandler.Handle(
            new TrackAnalyticsTelemetryCommand(command.EventName, AnalyticsTelemetryType.Event, tags.ToArray()),
            cancellationToken
        );

        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        return Result.Success();
    }

    private static List<(string Property, string Value)> BuildTags(
        string pageId,
        string ctaType,
        string? selectedAnswer,
        FeedbackTelemetryContext context
    )
    {
        var tags = new List<(string Property, string Value)>
        {
            (AnalyticsTagNames.PageId, pageId),
            (AnalyticsTagNames.CtaType, ctaType),
            (AnalyticsTagNames.ExperimentName, AnalyticsExperiments.FeedbackBannerAdditive),
            (AnalyticsTagNames.TreatmentGroup, context.TreatmentGroup),
            (AnalyticsTagNames.BannerShown, context.BannerShown ? "true" : "false"),
            (AnalyticsTagNames.Client, "web"),
        };

        if (!string.IsNullOrWhiteSpace(selectedAnswer))
        {
            tags.Add((AnalyticsTagNames.SelectedAnswer, selectedAnswer));
        }

        if (!string.IsNullOrWhiteSpace(context.UserId))
        {
            tags.Add((AnalyticsTagNames.UserId, context.UserId));
        }

        if (!string.IsNullOrWhiteSpace(context.OrganisationId))
        {
            tags.Add((AnalyticsTagNames.OrganisationId, context.OrganisationId));
        }

        if (!string.IsNullOrWhiteSpace(context.SessionId))
        {
            tags.Add((AnalyticsTagNames.SessionId, context.SessionId));
        }

        return tags;
    }
}
