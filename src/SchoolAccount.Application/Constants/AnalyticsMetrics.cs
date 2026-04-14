namespace SchoolAccount.Application.Constants;

public static class AnalyticsMetrics
{
    public const string UserLogin = "user_login";
    public const string FeatureUsed = "feature_used";
    public const string WorkflowStarted = "workflow_started";
    public const string WorkflowCompleted = "workflow_completed";
}

public static class AnalyticsTagNames
{
    public const string Outcome = "Outcome";
    public const string AuthMethod = "AuthMethod";
    public const string Journey = "Journey";
    public const string UserId = "UserId";
    public const string Scheme = "Scheme";
    public const string Client = "Client";
}

public static class MeterConstants
{
    public const string SchoolAccountAnalytics = "SchoolAccount.Analytics";
    public const string SchoolAccountFeedback = "SchoolAccount.Feedback";
}
