using System.Text;

namespace SchoolAccount.Web.Connect;

public static class RouteConstants
{
    public const string Root = "/";
    public const string Maintenance = Root + "maintenance";
    public const string Support = Root + "support";
    public const string Cookies = Root + "cookies";
    public const string Error = Root + "error/{code}";
    public const string FeedBack = Root + "feedback/page-useful";
    public const string FeedBackExit = "/feedback/exit";
    public const string ContactUs = "https://customerhelpportal.education.gov.uk/";
    public const string PrivacyPolicy =
        "https://www.gov.uk/government/publications/privacy-information-education-providers-workforce-including-teachers/5a254207-a566-44f7-ac77-6ba59fd26e04";

    public const string AccessibilityStatement = "https://accessibility-statements.education.gov.uk/s/21";

    public static class Start
    {
        public const string Index = Root + "start";
        public const string MatAcceptance = Index + "/mat";
        public const string SelectAOrganisation = Index + "/organisation";
        public const string PickAOrganisation = Index + "/organisation/{type}/{ukprn}";
        public const string ReturnToTrust = Index + "/organisation/return-to-trust";
    }

    public static class Calendar
    {
        public const string Index = Root + "calendar";
    }

    public static class Category
    {
        public const string Index = Root + "categories";
        public const string Hub = Root + "categories/{id}";
        public const string AllTasks = Root + "categories/all-tasks";
    }

    public static class Task
    {
        public static readonly CompositeFormat Index = CompositeFormat.Parse(Root + "task/{0}");
    }
}
