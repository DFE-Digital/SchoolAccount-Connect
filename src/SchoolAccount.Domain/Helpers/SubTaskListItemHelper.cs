using SchoolAccount.Domain.Dtos;
using SchoolAccount.Domain.Entities;

namespace SchoolAccount.Domain.Helpers
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
                subTaskEntity.RequirementId,
                subTaskEntity.StartDateIsExact,
                subTaskEntity.DueDateIsExact,
                subTaskEntity.WorkflowStateId
            );
        }
    }
}
