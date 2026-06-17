namespace SchoolAccount.Web.Connect.Features.Tasks;

internal static class ViewAddressConstants
{
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
}
