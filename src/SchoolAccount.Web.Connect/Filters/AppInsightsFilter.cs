using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Constants;
using SchoolAccount.Application.Features.Telemetry.Commands;
using SchoolAccount.Application.Features.Telemetry.Enums;
using SchoolAccount.Kernel;
using SchoolAccount.Web.Connect.Telemetry;

namespace SchoolAccount.Web.Connect.Filters;

[SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase")]
public sealed class AppInsightsFilter(
    ICommandHandler<TrackAnalyticsTelemetryCommand> telemetryCommandHandler,
    IOrganisationContext organisationContext
) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        await next();

        var httpContext = context.HttpContext;
        var user = httpContext.User;

        if (!HttpMethods.IsGet(httpContext.Request.Method))
        {
            return;
        }

        if (user.Identity?.IsAuthenticated != true)
        {
            return;
        }

        var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
        if (descriptor is null)
        {
            return;
        }

        var pageId = BuildPageId(descriptor);
        var feature = BuildFeature(descriptor);
        var journey = BuildJourney(descriptor);

        var hashedUserId = TryGetHashedUserId(user);
        var organisationId = GetOrganisationId(organisationContext);
        var sessionId = GetSessionId(user);

        var metricCommand = new TrackAnalyticsTelemetryCommand(
            AnalyticsMetrics.ConnectJourney,
            AnalyticsTelemetryType.Metric,
            (AnalyticsTagNames.EventName, AnalyticsEvents.PageVisited),
            (AnalyticsTagNames.PageId, pageId),
            (AnalyticsTagNames.Feature, feature),
            (AnalyticsTagNames.Journey, journey),
            (AnalyticsTagNames.Client, "web")
        );

        await telemetryCommandHandler.Handle(metricCommand, httpContext.RequestAborted);

        var eventTags = new List<(string Property, string Value)>
        {
            (AnalyticsTagNames.PageId, pageId),
            (AnalyticsTagNames.Feature, feature),
            (AnalyticsTagNames.Journey, journey),
            (AnalyticsTagNames.Client, "web"),
        };

        if (!string.IsNullOrWhiteSpace(hashedUserId))
        {
            eventTags.Add((AnalyticsTagNames.UserId, hashedUserId));
        }

        if (!string.IsNullOrWhiteSpace(organisationId))
        {
            eventTags.Add((AnalyticsTagNames.OrganisationId, organisationId));
        }

        if (!string.IsNullOrWhiteSpace(sessionId))
        {
            eventTags.Add((AnalyticsTagNames.SessionId, sessionId));
        }

        var eventCommand = new TrackAnalyticsTelemetryCommand(
            AnalyticsEvents.PageVisited,
            AnalyticsTelemetryType.Event,
            eventTags.ToArray()
        );

        await telemetryCommandHandler.Handle(eventCommand, httpContext.RequestAborted);
    }

    private static string BuildPageId(ControllerActionDescriptor descriptor)
    {
        return $"{descriptor.ControllerName}_{descriptor.ActionName}".ToLowerInvariant();
    }

    private static string BuildFeature(ControllerActionDescriptor descriptor)
    {
        return descriptor.ControllerName.ToLowerInvariant();
    }

    private static string BuildJourney(ControllerActionDescriptor descriptor)
    {
        return descriptor.ControllerName.ToLowerInvariant();
    }

    private static string? TryGetHashedUserId(ClaimsPrincipal user)
    {
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
        if (!organisationContext.IsAuthorised || !organisationContext.IsDsiDetermined)
        {
            return null;
        }

        var ukrpn = organisationContext.Organisation?.Ukrpn;

        return string.IsNullOrWhiteSpace(ukrpn) ? null : ukrpn;
    }

    private static string? GetSessionId(ClaimsPrincipal user)
    {
        return AnalyticsSessionIdProvider.GetSessionId(user);
    }
}
