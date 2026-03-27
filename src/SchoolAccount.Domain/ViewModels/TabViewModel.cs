namespace SchoolAccount.Domain.ViewModels
{
    public class TabViewModel
    {
        public ICollection<SubTaskViewModel> SelectedTabSubTasks { get; }
        public string NoTasksMessage { get; }
        public MenuItemViewModel PreviousTab { get; set; }
        public MenuItemViewModel UpcomingTab { get; set; }
        public string Path { get; set; } = string.Empty;

        public TabViewModel(TaskDetailViewMode tabIndex, ICollection<SubTaskViewModel>? selectedTabSubTasks, long id)
        {
            SelectedTabSubTasks = selectedTabSubTasks ?? [];
            if (tabIndex == TaskDetailViewMode.UpcomingTasks)
            {
                NoTasksMessage = "There are no upcoming tasks.";
            }
            else
            {
                NoTasksMessage = "There are no previous tasks.";
            }
            PreviousTab = new MenuItemViewModel(
                tabIndex == TaskDetailViewMode.PreviousTasks,
                $"{Path}?TaskId={id}&TabIndex={TaskDetailViewMode.PreviousTasks}",
                "Previous 12 months"
            );
            UpcomingTab = new MenuItemViewModel(
                tabIndex == TaskDetailViewMode.UpcomingTasks,
                $"{Path}?TaskId={id}&TabIndex={TaskDetailViewMode.UpcomingTasks}",
                "Upcoming tasks"
            );
        }
    }
}
