using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Features.Telemetry.Enums;

namespace SchoolAccount.Application.Features.Telemetry.Commands;

public sealed record TrackAnalyticsTelemetryCommand : ICommand
{
    public string Name { get; }
    public AnalyticsTelemetryType Type { get; }
    public IReadOnlyCollection<(string Property, string Value)> Tags { get; }

    public TrackAnalyticsTelemetryCommand(
        string name,
        AnalyticsTelemetryType type,
        params (string Property, string Value)[] tags
    )
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        Name = name;
        Type = type;
        Tags = tags;
    }
}
