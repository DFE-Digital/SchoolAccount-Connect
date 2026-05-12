using System.Security.Claims;
using Microsoft.FeatureManagement;
using SchoolAccount.Application.Constants;
using SchoolAccount.Application.Features.Feedback;
using SchoolAccount.Kernel;

namespace SchoolAccount.Web.Connect.Telemetry;

public sealed class FeedbackTelemetryContextProvider(
    IHttpContextAccessor httpContextAccessor,
    IOrganisationContext organisationContext,
    IFeatureManager featureManager
) : IFeedbackTelemetryContextProvider
{
    public FeedbackTelemetryContext GetContext()
    {
        var httpContext = httpContextAccessor.HttpContext;
        var user = httpContext?.User;

        var bannerShown = featureManager.IsEnabledAsync(FeatureFlagConstants.FeedbackBanner).GetAwaiter().GetResult();

        var treatmentGroup = bannerShown
            ? AnalyticsTreatmentGroups.YesNoPlusBanner
            : AnalyticsTreatmentGroups.YesNoOnly;

        return new FeedbackTelemetryContext(
            treatmentGroup,
            bannerShown,
            TryGetHashedUserId(user),
            GetOrganisationId(organisationContext),
            GetSessionId(user)
        );
    }

    private static string? TryGetHashedUserId(ClaimsPrincipal? user)
    {
        if (user is null)
        {
            return null;
        }

        try
        {
            return TelemetryUserIdFactory.CreateHashedUserId(user);
        }
        catch (InvalidOperationException)
        {
            return null;
        }
    }

    private static string? GetOrganisationId(IOrganisationContext organisationContext)
    {
        if (!organisationContext.IsAuthorised || !organisationContext.IsValid)
        {
            return null;
        }

        var ukrpn = organisationContext.Organisation?.Ukrpn;

        return string.IsNullOrWhiteSpace(ukrpn) ? null : ukrpn;
    }

    private static string? GetSessionId(ClaimsPrincipal? user)
    {
        return user is null ? null : AnalyticsSessionIdProvider.GetSessionId(user);
    }
}
