namespace SchoolAccount.Web.Connect;

public static class RouteConstants
{
    public const string Root = "/";
    public const string Maintenance = Root + "maintenance";
    public const string Support = Root + "support";
    public const string Error = Root + "error/{code}";
    public const string FeedBack = Root + "feedback/page-useful";

    internal static class Start
    {
        public const string Index = Root + "start";
        public const string MatAcceptance = Index + "/mat";
    }

    internal static class Calendar
    {
        public const string Index = Root + "calendar";
    }
}
