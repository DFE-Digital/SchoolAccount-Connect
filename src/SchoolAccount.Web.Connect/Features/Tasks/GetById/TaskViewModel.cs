using SchoolAccount.Application.Features.Tasks.GetById;
using static SchoolAccount.Domain.Common.WorkflowState;
using static SchoolAccount.Web.Connect.Features.Tasks.GetById.TaskViewMode;

namespace SchoolAccount.Web.Connect.Features.Tasks.GetById;

public enum TaskViewMode
{
    UpcomingTasks,
    PreviousTasks,
}

public sealed record TaskViewModel(GetTaskByIdResponse TaskResponse, TaskViewMode ViewMode = UpcomingTasks)
{
    public GetTaskByIdResponse Task => TaskResponse;

    public IReadOnlyCollection<GetTaskByIdResponseResource> ResourcesWithALink =>
        Task.Resources.Where(r => r.HasLink).ToArray();

    public IReadOnlyCollection<GetTaskByIdResponseSubtask> SubTasks => GetSubTasksForViewMode();

    public bool HasResources => Task.Resources.Count > 0;

    public bool HasRelatedTasks => Task.RelatedTasks.Count > 0;

    public bool IsUpcomingTasksView => ViewMode == UpcomingTasks;

    public bool IsPreviousTasksView => ViewMode == PreviousTasks;

    public string HeadingText => IsUpcomingTasksView ? "Upcoming tasks" : "Previous 12 months";

    public string CurrentlyActiveTabAccessibilityLabel => $"Currently active tab is {HeadingText}";

    public string NoTasksFoundMessage =>
        IsUpcomingTasksView ? "There are no upcoming tasks." : "There are no previous tasks.";

    private GetTaskByIdResponseSubtask[] GetSubTasksForViewMode()
    {
        var subtasks = IsUpcomingTasksView
            ? Task.SubTasks.Where(subtask => subtask.WorkflowState == Published)
            : Task.SubTasks.Where(subtask => subtask.WorkflowState == Expired);

        return subtasks
            .OrderBy(st => st.SortingDate.HasValue ? 0 : 1) // Nulls last
            .ThenBy(st => st.SortingDate)
            .ToArray();
    }
}
