namespace SchoolAccount.Web.Connect.Models;

public sealed record FeedbackTelemetry(
    string PageId,
    string Value,
    string Variant,
    string Action,
    string UserId,
    string OrganisationId,
    string OrganisationType,
    string Establishment,
    string Category,
    string Provider,
    string Region,
    string LocalAuthority,
    string LegalName);