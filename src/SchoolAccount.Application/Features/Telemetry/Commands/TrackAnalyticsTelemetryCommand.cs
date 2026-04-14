using SchoolAccount.Application.Abstractions.Messaging;

namespace SchoolAccount.Application.Features.Telemetry.Commands;

public sealed record TrackAnalyticsTelemetryCommand : ICommand
{
    public string Metric { get; }
    public IReadOnlyCollection<(string Property, string Value)> Tags { get; }

    public TrackAnalyticsTelemetryCommand(string metric, params (string Property, string Value)[] tags)
    {
        Metric = metric;
        Tags = tags;
    }
}
