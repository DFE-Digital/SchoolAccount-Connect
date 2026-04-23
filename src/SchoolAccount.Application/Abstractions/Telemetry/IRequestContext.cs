namespace SchoolAccount.Application.Abstractions.Telemetry;

public interface IRequestContext
{
    string? TraceId { get; }
}
