namespace SchoolAccount.Web.Connect.Features.CalendarOfItems;

internal static class CalendarOfItemsConstants
{
    public static class Routes
    {
        private const string Index = RouteConstants.Root + "calendar";
        public const string Query = Index;
    }

    public static class Views
    {
        public const string Query = "~/Features/CalendarOfItems/Query/Query.cshtml";
        public const string Widget = "~/Features/CalendarOfItems/Shared/Widget.cshtml";
        public const string Tab = "~/Features/CalendarOfItems/Shared/Tab.cshtml";
    }
}
