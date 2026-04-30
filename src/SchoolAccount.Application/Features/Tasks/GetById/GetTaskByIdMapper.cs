using SchoolAccount.Domain.Resources;
using SchoolAccount.Domain.Subtasks;
using SchoolAccount.Domain.Tasks;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Features.Tasks.GetById;

public class GetTaskByIdMapper(IDateTimeProvider dateTimeProvider)
{
    public TaskResponse ToTaskResponse(TaskEntity task, TaskViewMode viewMode)
    {
        return new TaskResponse
        {
            Id = task.Id,
            ReferenceNo = task.ReferenceNo,
            Name = task.Name,
            SubTaskLastUpdated = task.SubTaskLastUpdated,
            ViewMode = viewMode,
            TotalSubTasks = task.SubTasks.Count,
            Requirement = task.Requirement,
            DateUpdated = task.DateUpdated,
            UpdatedBy = task.UpdatedBy,
            UpcomingSubTasks = task.PublishedSubTasks.Select(ToTaskResponseSubTasks),
            PreviousSubTasks = task.ExpiredSubTasks.Select(ToTaskResponseSubTasks),
            Resources = task.Resources.Select(ToTaskResponseResources),
        };
    }

    private TaskResponseSubTask ToTaskResponseSubTasks(SubTaskEntity subTask)
    {
        return new TaskResponseSubTask
        {
            Id = subTask.Id,
            ReferenceNo = subTask.ReferenceNo,
            Name = subTask.Name,
            Description = subTask.Description,
            DigitalLink = subTask.DigitalTaskLink,
            StartDate = subTask.StartDate,
            StartDateIsExact = subTask.StartDateIsExact,
            DueDate = subTask.DueDate,
            DueDateIsExact = subTask.DueDateIsExact,
            AvailabilityLabel = subTask.GenerateAvailabilityLabel(dateTimeProvider),
            DueDateLabel = subTask.GenerateDueDateLabel(),
            Requirement = subTask.Requirement,
            WorkflowState = subTask.WorkflowState,
            DateUpdated = subTask.DateUpdated,
            UpdatedBy = subTask.UpdatedBy,
            HasDescription = subTask.HasDescription,
            HasLink = subTask.HasLink,
            IsOptional = subTask.IsOptional,
        };
    }

    private TaskResponseResource ToTaskResponseResources(ResourceEntity resource)
    {
        return new TaskResponseResource { Name = resource.ResourceName, Link = resource.DigitalLink };
    }
}
