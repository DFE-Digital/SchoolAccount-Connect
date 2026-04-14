using System.Collections.Concurrent;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Metrics;
using Microsoft.Extensions.Logging;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Abstractions.Telemetry;
using SchoolAccount.Application.Constants;
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

    public Task<Result> Handle(TrackAnalyticsTelemetryCommand command, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(command.Metric);

        var traceId = requestContext.TraceId;

        var counter = Counters.GetOrAdd(command.Metric, static metric => Meter.CreateCounter<int>(metric));

        var tags = new TagList();
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

            if (string.Equals(property, AnalyticsTagNames.Client, StringComparison.OrdinalIgnoreCase))
            {
                hasClientTag = true;
            }

            tags.Add(property, value);
        }

        if (!hasClientTag)
        {
            tags.Add(AnalyticsTagNames.Client, "web");
        }

        counter.Add(1, tags);

        logger.LogInformation(
            "Analytics event {Metric} tracked with TraceId {TraceId} and tags {@Tags}",
            command.Metric,
            traceId,
            command.Tags
        );

        return Task.FromResult(Result.Success());
    }
}
