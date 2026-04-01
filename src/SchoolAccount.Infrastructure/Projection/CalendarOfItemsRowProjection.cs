using System.Linq.Expressions;
using SchoolAccount.Application.Features.CalendarOfItems.Enums;
using SchoolAccount.Application.Features.CalendarOfItems.Models;
using SchoolAccount.Domain.Entities;

namespace SchoolAccount.Infrastructure.Projection;

public static class CalendarOfItemsRowProjection
{
    public static Expression<Func<SubTaskEntity, CalendarOfItemsRow>> FromSubTask()
    {
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
                DisplayValue = x.WorkflowState.Name,
                Type = CalendarOfItemsRowType.WorkflowState,
                EntityId = x.WorkflowStateId,
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
        };
    }
}
