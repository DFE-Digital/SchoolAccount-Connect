namespace SchoolAccount.Application.Constants;

public static class AnalyticsEvents
{
    public const string LoginSucceeded = "connect_login_succeeded";
    public const string LoginFailed = "connect_login_failed";
    public const string PageVisited = "connect_page_visited";

    public const string BannerExposureAssigned = "connect_banner_exposure_assigned";
    public const string CtaYesNoInteraction = "connect_cta_yes_no_interaction";
    public const string CtaFeedbackExit = "connect_cta_feedback_exit";
    public const string CtaCancelled = "connect_cta_cancelled";
    public const string CtaDismissed = "connect_cta_dismissed";
}
