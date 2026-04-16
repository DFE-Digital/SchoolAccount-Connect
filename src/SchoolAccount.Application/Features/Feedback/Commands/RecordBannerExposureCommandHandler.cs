using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Constants;
using SchoolAccount.Application.Features.Telemetry.Commands;
using SchoolAccount.Application.Features.Telemetry.Enums;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Features.Feedback.Commands;

public sealed class RecordBannerExposureCommandHandler(
    ICommandHandler<TrackAnalyticsTelemetryCommand> telemetryCommandHandler,
    IFeedbackTelemetryContextProvider feedbackTelemetryContextProvider
) : ICommandHandler<RecordBannerExposureCommand>
{
    public async Task<Result> Handle(RecordBannerExposureCommand command, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(command.PageId))
        {
            return Result.Failure(Error.Problem("Feedback.PageIdRequired", "Page id is required."));
        }

        var context = feedbackTelemetryContextProvider.GetContext();

        if (!context.BannerShown)
        {
            return Result.Success();
        }

        var eventResult = await telemetryCommandHandler.Handle(
            new TrackAnalyticsTelemetryCommand(
                AnalyticsEvents.BannerExposureAssigned,
                AnalyticsTelemetryType.Event,
                BuildTags(command.PageId, context).ToArray()
            ),
            cancellationToken
        );

        if (eventResult.IsFailure)
        {
            return Result.Failure(eventResult.Error);
        }

        var metricResult = await telemetryCommandHandler.Handle(
            new TrackAnalyticsTelemetryCommand(
                AnalyticsMetrics.BannerExposure,
                AnalyticsTelemetryType.Metric,
                (AnalyticsTagNames.EventName, AnalyticsEvents.BannerExposureAssigned),
                (AnalyticsTagNames.PageId, command.PageId),
                (AnalyticsTagNames.CtaType, AnalyticsCtaTypes.Banner),
                (AnalyticsTagNames.ExperimentName, AnalyticsExperiments.FeedbackBannerAdditive),
                (AnalyticsTagNames.TreatmentGroup, context.TreatmentGroup),
                (AnalyticsTagNames.BannerShown, "true"),
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

    private static List<(string Property, string Value)> BuildTags(string pageId, FeedbackTelemetryContext context)
    {
        var tags = new List<(string Property, string Value)>
        {
            (AnalyticsTagNames.PageId, pageId),
            (AnalyticsTagNames.CtaType, AnalyticsCtaTypes.Banner),
            (AnalyticsTagNames.ExperimentName, AnalyticsExperiments.FeedbackBannerAdditive),
            (AnalyticsTagNames.TreatmentGroup, context.TreatmentGroup),
            (AnalyticsTagNames.BannerShown, "true"),
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
