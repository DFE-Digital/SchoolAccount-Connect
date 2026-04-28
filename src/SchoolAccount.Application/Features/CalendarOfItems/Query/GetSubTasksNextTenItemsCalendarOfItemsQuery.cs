using SchoolAccount.Application.Extensions;
using SchoolAccount.Application.Features.CalendarOfItems.Enums;
using SchoolAccount.Application.Features.CalendarOfItems.Query.Operational;
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
            $"No tasks coming up over the next 12 months"
        ) { }
}
