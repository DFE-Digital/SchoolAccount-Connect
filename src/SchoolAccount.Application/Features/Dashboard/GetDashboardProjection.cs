using System.Linq.Expressions;
using SchoolAccount.Domain.Subtasks;
using SchoolAccount.Domain.Types;

namespace SchoolAccount.Application.Features.Dashboard;

internal static class GetDashboardProjection
{
    public static Expression<Func<SubTaskEntity, GetDashboardResponseCalendarItem>> CalendarItem() =>
        x => new GetDashboardResponseCalendarItem
        {
            Id = x.Task.Id,
            Name = x.Task.Name,
            Description = x.Name,
            StartDate = x.StartDate,
            StartDateIsExact = x.StartDateIsExact,
            DueDate = x.DueDate,
            DueDateIsExact = x.DueDateIsExact,
            SortDate = x.DueDate ?? x.StartDate,
            LastUpdated = x.DateUpdated,
        };

    public static Expression<Func<TypeEntity, GetDashboardResponseCategoryItem>> Category() =>
        subTaskEntity => new GetDashboardResponseCategoryItem
        {
            Id = subTaskEntity.Id,
            DisplayName = subTaskEntity.DisplayName,
            Description = subTaskEntity.Description,
        };
}
