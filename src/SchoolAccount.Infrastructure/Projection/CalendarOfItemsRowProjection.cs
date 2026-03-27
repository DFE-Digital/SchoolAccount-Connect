using System.Linq.Expressions;
using SchoolAccount.Application.Features.CalendarOfItems.Models;
using SchoolAccount.Infrastructure.Models.Entities;
using SchoolAccount.Application.Features.CalendarOfItems.Enums;

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
        };
    }
}
