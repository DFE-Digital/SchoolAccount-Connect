using System.Globalization;
using System.Text;

namespace SchoolAccount.Web.Connect;

public static class RouteConstants
{
    public static string Url<T>(CompositeFormat format, T id) =>
        string.Format(CultureInfo.InvariantCulture, format, id);

    public const string Root = "/";
    public const string Maintenance = Root + "maintenance";
    public const string Support = Root + "support";
    public const string About = Root + "about";
    public const string Cookies = Root + "cookies";
    public const string Error = Root + "error/{code}";
    public const string Search = Root + "/search";

    public const string FeedBack = Root + "feedback/page-useful";
    public const string FeedBackRespond = Root + "feedback/respond";
    public const string FeedBackCancel = Root + "feedback/cancel";
    public const string FeedBackExit = Root + "feedback/exit";

    public const string ContactUs = "https://customerhelpportal.education.gov.uk/";

    public const string PrivacyPolicy =
        "https://www.gov.uk/government/publications/privacy-information-education-providers-workforce-including-teachers/5a254207-a566-44f7-ac77-6ba59fd26e04";

    public const string AccessibilityStatement = "https://accessibility-statements.education.gov.uk/s/21";
    public const string AcademyTrustHandbook = "https://www.gov.uk/government/publications/academy-trust-handbook";

    public static class Start
    {
        public const string Index = Root + "start";
        public const string MatAcceptance = Index + "/mat";
    }

    public static class Account
    {
        private const string Index = Root + "account";
        public const string Login = Index + "/login";
        public const string SignOut = Index + "/signout";
        public const string SignedOut = Index + "/signedout";
    }

    public static class Calendar
    {
        public const string CalendarOfItems = Root + "calendar";
    }

    public static class Category
    {
        public const string List = Root + "categories";
        public const string Hub = Root + "categories/{id:int}";
        public static readonly CompositeFormat Index = CompositeFormat.Parse(Root + "categories/{0}");
    }

    public static class Task
    {
        public const string AllTasks = Root + "tasks";
        public const string GetById = Root + "tasks/{id:long}";
        public static readonly CompositeFormat Index = CompositeFormat.Parse(Root + "tasks/{0}");
    }
}
