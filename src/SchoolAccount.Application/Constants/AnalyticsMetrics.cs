namespace SchoolAccount.Application.Constants;

public static class AnalyticsMetrics
{
    public const string UserLogin = "user_login";
    public const string FeatureUsed = "feature_used";
    public const string WorkflowStarted = "workflow_started";
    public const string WorkflowCompleted = "workflow_completed";
    public const string ConnectJourney = "connect_user_journey";
}

public static class AnalyticsTagNames
{
    public const string EventName = "EventName";
    public const string Client = "Client";
    public const string Outcome = "Outcome";
    public const string FailureReason = "FailureReason";
    public const string AuthMethod = "AuthMethod";
    public const string Journey = "Journey";
    public const string Feature = "Feature";
    public const string PageId = "PageId";
    public const string UserId = "UserId";
    public const string OrganisationId = "OrganisationId";
    public const string SessionId = "SessionId";
    public const string Scheme = "Scheme";
}

public static class AnalyticsEvents
{
    public const string LoginSucceeded = "connect_login_succeeded";
    public const string LoginFailed = "connect_login_failed";
    public const string PageVisited = "connect_page_visited";
}

public static class MeterConstants
{
    public const string SchoolAccountAnalytics = "SchoolAccount.Analytics";
    public const string SchoolAccountFeedback = "SchoolAccount.Feedback";
}

public static class AnalyticsClaimTypes
{
    public const string SessionId = "analytics_session_id";
}
