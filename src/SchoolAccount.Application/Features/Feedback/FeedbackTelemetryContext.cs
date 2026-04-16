namespace SchoolAccount.Application.Features.Feedback;

public sealed record FeedbackTelemetryContext(
    string TreatmentGroup,
    bool BannerShown,
    string? UserId,
    string? OrganisationId,
    string? SessionId
);
