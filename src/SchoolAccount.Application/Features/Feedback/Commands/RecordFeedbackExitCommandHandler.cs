using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Constants;
using SchoolAccount.Application.Features.Telemetry.Commands;
using SchoolAccount.Application.Features.Telemetry.Enums;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Features.Feedback.Commands;

public sealed class RecordFeedbackExitCommandHandler(
    ICommandHandler<TrackAnalyticsTelemetryCommand> telemetryCommandHandler,
    IFeedbackTelemetryContextProvider feedbackTelemetryContextProvider
) : ICommandHandler<RecordFeedbackExitCommand>
{
    public async Task<Result> Handle(RecordFeedbackExitCommand command, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(command.PageId))
        {
            return Result.Failure(Error.Problem("Feedback.PageIdRequired", "Page id is required."));
        }

        if (string.IsNullOrWhiteSpace(command.CtaType))
        {
            return Result.Failure(Error.Problem("Feedback.CtaTypeRequired", "CTA type is required."));
        }

        var context = feedbackTelemetryContextProvider.GetContext();

        var eventResult = await telemetryCommandHandler.Handle(
            new TrackAnalyticsTelemetryCommand(
                AnalyticsEvents.CtaFeedbackExit,
                AnalyticsTelemetryType.Event,
                BuildTags(command.PageId, command.CtaType, context).ToArray()
            ),
            cancellationToken
        );

        if (eventResult.IsFailure)
        {
            return Result.Failure(eventResult.Error);
        }

        var metricResult = await telemetryCommandHandler.Handle(
            new TrackAnalyticsTelemetryCommand(
                AnalyticsMetrics.FeedbackResponse,
                AnalyticsTelemetryType.Metric,
                (AnalyticsTagNames.EventName, AnalyticsEvents.CtaFeedbackExit),
                (AnalyticsTagNames.PageId, command.PageId),
                (AnalyticsTagNames.CtaType, command.CtaType),
                (AnalyticsTagNames.ExperimentName, AnalyticsExperiments.FeedbackBannerAdditive),
                (AnalyticsTagNames.TreatmentGroup, context.TreatmentGroup),
                (AnalyticsTagNames.BannerShown, context.BannerShown ? "true" : "false"),
                (AnalyticsTagNames.Client, "web")
            ),
            cancellationToken
        );

        if (metricResult.IsFailure)
        {
            return Result.Failure(metricResult.Error);
        }

        return Result.Success();
    }

    private static List<(string Property, string Value)> BuildTags(
        string pageId,
        string ctaType,
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
