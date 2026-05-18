using System.Linq.Expressions;
using SchoolAccount.Application.Features.CalendarOfItems.Enums;
using SchoolAccount.Application.Features.CalendarOfItems.Models;
using SchoolAccount.Domain.SchoolTypes;
using SchoolAccount.Domain.Subtasks;
using SchoolAccount.Domain.Tasks;
using SchoolAccount.Kernel;

namespace SchoolAccount.Infrastructure.Projection;

public static class CalendarOfItemsRowProjection
{
    public static Expression<Func<SubTaskEntity, CalendarOfItemsRow>> FromSubTask(
        IQueryable<SchoolTypeTagMappingEntity> schoolTypeMappings,
        IEnumerable<SchoolType> types
    )
    {
        var typeIds = types.Select(t => (int)t).ToList();
        return x => new CalendarOfItemsRow
        {
            Id = x.Task.Id,
            Name = x.Task.Name,
            Description = x.Name,
            StartDate = x.StartDate,
            StartDateIsExact = x.StartDateIsExact,
            DueDate = x.DueDate,
            DueDateIsExact = x.DueDateIsExact,
            SortDate = x.DueDate ?? x.StartDate,
            Type = CalendarOfItemsRowType.Task,
            Status = new CalendarOfItemsRowStatus
            {
                DisplayValue = Enum.GetName(x.WorkflowState)!,
                Type = CalendarOfItemsRowType.WorkflowState,
                EntityId = (int)x.WorkflowState,
            },
            LastUpdated = x.DateUpdated,
            Tags = x.TagsSourceMappings.Select(t => new CalendarOfItemsExtensionNode
            {
                Id = t.Tag.Id,
                Name = t.Tag.Name,
                DisplayValue = t.Tag.Name,
                Type = CalendarOfItemsExtensionNodeType.Tag,
            }),
            Types = x.Task.TypeTaskMappings.Select(t => new CalendarOfItemsExtensionNode
            {
                Id = t.Type.Id,
                Name = t.Type.Name,
                DisplayValue = t.Type.Name,
                Type = CalendarOfItemsExtensionNodeType.Type,
            }),
            Organisations = x.TagsSourceMappings
                .SelectMany(t => schoolTypeMappings
                    .Where(st => typeIds.Contains(st.SchoolTypeId) && st.TagId == t.TagId)
                    .Select(st => new CalendarOfItemsExtensionNode
                    {
                        DisplayValue = st.SchoolType.Name,
                        Id = st.SchoolTypeId
                    }))
        };
    }

    public static Expression<Func<TaskEntity, CalendarOfItemsRow>> FromTask()
    {
        return x => new CalendarOfItemsRow
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            SortDate = x.SubTasks.Select(sb => sb.StartDate).Min() ?? x.SubTasks.Select(sb => sb.DueDate).Max(),
            Type = CalendarOfItemsRowType.Task,
            Status = new CalendarOfItemsRowStatus
            {
                DisplayValue = Enum.GetName(x.WorkflowState)!,
                Type = CalendarOfItemsRowType.WorkflowState,
                EntityId = (int)x.WorkflowState,
            },
            LastUpdated = x.DateUpdated,
        };
    }
}
