using SchoolAccount.Domain.Dtos;

namespace SchoolAccount.Domain.Subtasks
{
    public static class SubTaskListItemHelper
    {
        public static SubTaskListItemDto ToListItem(SubTaskEntity subTaskEntity)
        {
            return new SubTaskListItemDto(
                subTaskEntity.Id,
                subTaskEntity.ReferenceNo ?? string.Empty,
                subTaskEntity.Name ?? subTaskEntity.ReferenceNo ?? string.Empty,
                subTaskEntity.Description ?? string.Empty,
                subTaskEntity.DigitalTaskLink,
                subTaskEntity.UpdatedBy,
                subTaskEntity.StartDate,
                subTaskEntity.DateUpdated,
                subTaskEntity.DueDate,
                subTaskEntity.Requirement,
                subTaskEntity.StartDateIsExact,
                subTaskEntity.DueDateIsExact,
                subTaskEntity.WorkflowState
            );
        }
    }
}
