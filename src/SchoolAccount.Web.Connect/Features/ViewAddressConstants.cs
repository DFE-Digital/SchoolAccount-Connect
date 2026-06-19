namespace SchoolAccount.Web.Connect.Features;

internal static class ViewAddressConstants
{
    internal static class Home
    {
        public const string Cookies = "~/Features/Home/Cookies/Cookies.cshtml";
        public const string Dashboard = "~/Features/Home/Dashboard/Dashboard.cshtml";
        public const string Maintenance = "~/Features/Home/Maintenance/Maintenance.cshtml";
        public const string Support = "~/Features/Home/Support/Support.cshtml";
    }

    internal static class Tasks
    {
        public const string GetAll = "~/Features/Tasks/GetAll/AllTasks.cshtml";
        public const string GetById = "~/Features/Tasks/GetById/Task.cshtml";
        public const string Search = "~/Features/Tasks/Search/SearchTasks.cshtml";

        internal static class Partials
        {
            public const string Tabs = "~/Features/Tasks/GetById/_Tabs.cshtml";
            public const string Guidance = "~/Features/Tasks/GetById/_Guidance.cshtml";
            public const string Subtasks = "~/Features/Tasks/GetById/_Subtasks.cshtml";
            public const string RelatedTasks = "~/Features/Tasks/GetById/_RelatedTasks.cshtml";
        }
    }

    internal static class Categories
    {
        public const string Hub = "~/Features/Categories/CategoryHub/CategoryHub.cshtml";
        public const string List = "~/Features/Categories/CategoryList/CategoryList.cshtml";
    }

    internal static class Shared
    {
        public const string PaginatedList = "~/Features/Shared/ListItem/_PaginatedList.cshtml";
        public const string Pagination = "~/Features/Shared/ListItem/_Pagination.cshtml";
        public const string ListItem = "~/Features/Shared/ListItem/_ListItem.cshtml";
        public const string Arrow = "~/Views/Shared/SVG/ArrowRight.cshtml";
    }
}
