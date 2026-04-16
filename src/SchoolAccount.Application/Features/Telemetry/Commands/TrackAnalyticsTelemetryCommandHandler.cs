using System.Collections.Concurrent;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Metrics;
using Microsoft.Extensions.Logging;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Abstractions.Telemetry;
using SchoolAccount.Application.Constants;
using SchoolAccount.Application.Features.Telemetry.Enums;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Features.Telemetry.Commands;

[SuppressMessage("Performance", "CA1873:Avoid potentially expensive logging")]
public sealed class TrackAnalyticsTelemetryCommandHandler(
    IRequestContext requestContext,
    ILogger<TrackAnalyticsTelemetryCommandHandler> logger
) : ICommandHandler<TrackAnalyticsTelemetryCommand>
{
    private static readonly Meter Meter = new(MeterConstants.SchoolAccountAnalytics);
    private static readonly ConcurrentDictionary<string, Counter<int>> Counters = new();

    private static readonly HashSet<string> BlockedMetricTags =
    [
        AnalyticsTagNames.UserId,
        AnalyticsTagNames.OrganisationId,
        AnalyticsTagNames.SessionId,
    ];

    public Task<Result> Handle(TrackAnalyticsTelemetryCommand command, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(command.Name);

        return command.Type switch
        {
            AnalyticsTelemetryType.Metric => HandleMetric(command),
            AnalyticsTelemetryType.Event => HandleEvent(command),
            _ => Task.FromResult(
                Result.Failure(Error.Problem("Telemetry.InvalidType", $"Unsupported telemetry type '{command.Type}'."))
            ),
        };
    }

    private Task<Result> HandleMetric(TrackAnalyticsTelemetryCommand command)
    {
        var traceId = requestContext.TraceId;

        var counter = Counters.GetOrAdd(command.Name, static metric => Meter.CreateCounter<int>(metric));

        var metricTags = new TagList();
        var logProperties = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        var hasClientTag = false;
        var seenKeys = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var (property, value) in command.Tags)
        {
            if (string.IsNullOrWhiteSpace(property) || string.IsNullOrWhiteSpace(value))
            {
                continue;
            }

            if (!seenKeys.Add(property))
            {
                continue;
            }

            logProperties[property] = value;

            if (string.Equals(property, AnalyticsTagNames.Client, StringComparison.OrdinalIgnoreCase))
            {
                hasClientTag = true;
            }

            if (BlockedMetricTags.Contains(property))
            {
                continue;
            }

            metricTags.Add(property, value);
        }

        if (!hasClientTag)
        {
            metricTags.Add(AnalyticsTagNames.Client, "web");
            logProperties.TryAdd(AnalyticsTagNames.Client, "web");
        }

        counter.Add(1, metricTags);

        logger.LogInformation(
            "Analytics event {EventName} tracked with TraceId {TraceId} and properties {@Properties}",
            command.Name,
            traceId,
            logProperties
        );

        return Task.FromResult(Result.Success());
    }

    private Task<Result> HandleEvent(TrackAnalyticsTelemetryCommand command)
    {
        var traceId = requestContext.TraceId;

        var properties = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        var hasClientTag = false;

        foreach (var (property, value) in command.Tags)
        {
            if (string.IsNullOrWhiteSpace(property) || string.IsNullOrWhiteSpace(value))
            {
                continue;
            }

            if (properties.ContainsKey(property))
            {
                continue;
            }

            properties[property] = value;

            if (string.Equals(property, AnalyticsTagNames.Client, StringComparison.OrdinalIgnoreCase))
            {
                hasClientTag = true;
            }
        }

        if (!hasClientTag)
        {
            properties[AnalyticsTagNames.Client] = "web";
        }

        properties["TraceId"] = traceId!;

        using (logger.BeginScope(properties))
        {
            logger.LogInformation("Analytics event {EventName} tracked", command.Name);
        }

        return Task.FromResult(Result.Success());
    }
}
