using SchoolAccount.Application.Extensions;
using SchoolAccount.Application.Features.CalendarOfItems.Enums;
using SchoolAccount.Application.Features.CalendarOfItems.Models;
using SchoolAccount.Application.Features.CalendarOfItems.Query.Operational;
using SchoolAccount.Application.Features.Shared.Filtering;
using SchoolAccount.Domain.Common;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Features.CalendarOfItems.Query;

public record GetSubTasksNextTenItemsCalendarOfItemsQuery : CalendarOfItemsCustomQuery
{
    public GetSubTasksNextTenItemsCalendarOfItemsQuery(DateOnly date)
        : base(
            CalendarOfItemsQueryTypes.SubTask,
            new DateOnlyRange(date.StartOfMonth(), date.AddMonths(12).EndOfMonth()),
            10,
            1,
            CalendarOfItemsSortMode.NotSpecified,
            $"No tasks coming up over the next 12 months",
            new CalendarOfItemsFilter([
                new FilterRequest
                {
                    Field = "state",
                    Operator = ComparisonType.Equals,
                    Value = WorkflowState.Published
                }
            ])
        ) { }
}
