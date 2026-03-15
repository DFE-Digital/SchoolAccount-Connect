namespace SchoolAccount.Application.Features.Feedback.Commands;

public sealed record FeedbackTelemetry(
    string PageId,
    string Value,
    string Variant,
    string Action,
    string OrganisationType,
    string Establishment,
    string Category,
    string Provider,
    string Region,
    string LocalAuthority,
    string Name);