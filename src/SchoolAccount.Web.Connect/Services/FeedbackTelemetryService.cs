using System.Security.Cryptography;
using System.Text;
using SchoolAccount.Kernel;
using SchoolAccount.Web.Connect.Models;

namespace SchoolAccount.Web.Connect.Services;

public class FeedbackTelemetryService(
    ILogger<FeedbackTelemetryService> logger,
    IUserContext userContext,
    IOrganisationContext organisationContext)
    : IFeedbackTelemetryService
{
    private const string EventName = "page_feedback_response";

    public void RecordPageFeedback(PageFeedbackRequest request)
    {
        var telemetry = BuildTelemetry(request);

        logger.LogInformation(
            "Feedback response captured. EventName: {EventName}, Variant: {Variant}, Value: {Value}, Action: {Action}, PageId: {PageId}, UserId: {UserId}, OrganisationId: {OrganisationId}, OrganisationType: {OrganisationType}, Establishment: {Establishment}, Category: {Category}, Provider: {Provider}",
            EventName,
            telemetry.Variant,
            telemetry.Value,
            telemetry.Action,
            telemetry.PageId,
            telemetry.UserId,
            telemetry.OrganisationId,
            telemetry.OrganisationType,
            telemetry.Establishment,
            telemetry.Category,
            telemetry.Provider);
    }

    private FeedbackTelemetry BuildTelemetry(PageFeedbackRequest request)
    {
        var organisationIdentifier = GetOrganisationIdentifier();

        return new FeedbackTelemetry(
            PageId: request.PageId.Trim(),
            Value: request.Value.Trim(),
            Variant: request.Variant.Trim(),
            Action: request.Action?.Trim() ?? "unknown",
            UserId: HashValue(userContext.Id),
            OrganisationId: HashValue(organisationIdentifier),
            OrganisationType: organisationContext.Type.ToString(),
            Establishment: organisationContext.Establishment.ToString(),
            Category: organisationContext.Category.ToString(),
            Provider: organisationContext.Provider.ToString() ?? "unknown");
    }

    private string? GetOrganisationIdentifier()
    {
        var ukprn = organisationContext.Organisation switch
        {
            Kernel.Organisations.AcademyOrganisation academy => academy.Ukrpn,
            Kernel.Organisations.TrustOrganisation trust => trust.Ukrpn,
            Kernel.Organisations.OtherOrganisation other => other.Ukrpn,
            _ => null
        };

        return ukprn;
    }

    private static string HashValue(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return "unknown";
        }

        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(value));
        return Convert.ToHexString(bytes);
    }
}