using SchoolAccount.Domain.Common;
using static SchoolAccount.Application.Features.Tasks.GetById.TaskViewMode;

namespace SchoolAccount.Application.Features.Tasks.GetById
{
    public enum TaskViewMode
    {
        UpcomingTasks,
        PreviousTasks,
    }

    public class TaskResponse
    {
        public long Id { get; init; }

        public string? ReferenceNo { get; init; }

        public string Name { get; init; } = string.Empty;

        public DateTime? SubTaskLastUpdated { get; init; }

        public TaskViewMode ViewMode { get; init; }

        public int TotalSubTasks { get; init; }

        public Requirement? Requirement { get; init; }

        public DateTime DateUpdated { get; init; }

        public string UpdatedBy { get; init; } = string.Empty;

        public IEnumerable<TaskResponseSubTask> UpcomingSubTasks { get; init; } = [];

        public IEnumerable<TaskResponseSubTask> PreviousSubTasks { get; init; } = [];

        public IEnumerable<TaskResponseSubTask> CurrentSubTasks =>
            ViewMode == UpcomingTasks ? UpcomingSubTasks : PreviousSubTasks;

        public IEnumerable<TaskResponseResource> Resources { get; init; } = [];

        public string HeadingText => ViewMode == UpcomingTasks ? "Upcoming Tasks" : "Previous 12 months";

        public string NoTasksFoundMessage =>
            ViewMode == UpcomingTasks ? "There are no upcoming tasks" : "There are no previous tasks";

        public bool IsUpcomingTasksView => ViewMode == UpcomingTasks;

        public bool IsPreviousTasksView => ViewMode == PreviousTasks;
    }

    public class TaskResponseSubTask
    {
        public long Id { get; init; }

        public string? ReferenceNo { get; init; }

        public string Name { get; init; } = string.Empty;

        public string? Description { get; init; }

        public string? DigitalLink { get; init; }

        public DateOnly? StartDate { get; init; }

        public bool? StartDateIsExact { get; init; }

        public DateOnly? DueDate { get; init; }

        public bool? DueDateIsExact { get; init; }

        public string AvailabilityLabel { get; init; } = string.Empty;

        public string DueDateLabel { get; init; } = string.Empty;

        public Requirement Requirement { get; init; }

        public WorkflowState WorkflowState { get; init; }

        public DateTime DateUpdated { get; init; }

        public string UpdatedBy { get; init; } = string.Empty;

        public bool HasDueDateLabel => !string.IsNullOrWhiteSpace(DueDateLabel);

        public bool HasAvailabilityLabel => !string.IsNullOrWhiteSpace(AvailabilityLabel);

        public bool HasDescription { get; init; }

        public bool HasLink { get; init; }

        public bool IsOptional { get; init; }

        public string? ResourceName { get; init; } = string.Empty;

        public string? ResourceLink { get; init; } = string.Empty;

        public bool HasResourceLink => !string.IsNullOrWhiteSpace(ResourceLink);
    }

    public class TaskResponseResource
    {
        public required string Name { get; init; }

        public string? Link { get; init; }

        public bool HasLink => !string.IsNullOrWhiteSpace(Link);
    }
}
