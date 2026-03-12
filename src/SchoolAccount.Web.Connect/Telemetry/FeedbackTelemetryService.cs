using System.Diagnostics;
using System.Diagnostics.Metrics;
using SchoolAccount.Application.Abstractions.Telemetry;
using SchoolAccount.Application.Features.Feedback.Commands;
using SchoolAccount.Kernel;
using SchoolAccount.Web.Connect.Extensions;

namespace SchoolAccount.Web.Connect.Telemetry;

public class FeedbackTelemetryService(IOrganisationContext organisationContext, IHttpContextAccessor contextAccessor) : IFeedbackTelemetryService
{
    private static readonly Meter Meter = new("SchoolAccount.Feedback");

    private static readonly Counter<int> FeedbackCounter =
        Meter.CreateCounter<int>("page_feedback_response");

    public void RecordPageFeedback(RecordPageFeedbackCommand command)
    {
        var telemetry = BuildTelemetry(command);

        var tags = new TagList
        {
            { "PageId", telemetry.PageId },
            { "Variant", telemetry.Variant },
            { "Value", telemetry.Value },
            { "Action", telemetry.Action },
            { "OrganisationType", telemetry.OrganisationType },
            { "Establishment", telemetry.Establishment },
            { "Category", telemetry.Category },
            { "Provider", telemetry.Provider },
            { "Region", telemetry.Region },
            { "LocalAuthority", telemetry.LocalAuthority }
        };

        FeedbackCounter.Add(1, tags);
    }

    private FeedbackTelemetry BuildTelemetry(RecordPageFeedbackCommand request)
    {
        var claim = contextAccessor.GetOrganisation();

        return new FeedbackTelemetry(
            PageId: request.PageId.Trim(),
            Value: request.Value.Trim(),
            Variant: request.Variant.Trim(),
            Action: request.Action?.Trim() ?? "unknown",
            OrganisationType: organisationContext.Type.ToString(),
            Establishment: organisationContext.Establishment.ToString(),
            Category: organisationContext.Category.ToString(),
            Provider: organisationContext.Provider.ToString() ?? "unknown",
            Region: CleanOrUnknown(claim?.Region?.Name),
            LocalAuthority: CleanOrUnknown(claim?.LocalAuthority?.Name));
    }

    private static string CleanOrUnknown(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? "unknown" : value.Trim();
    }
}