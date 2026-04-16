using SchoolAccount.Application.Extensions;
using SchoolAccount.Application.Features.CalendarOfItems.Enums;
using SchoolAccount.Application.Features.CalendarOfItems.Query.Operational;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Features.CalendarOfItems.Query;

public record GetSubTasksNextTenItemsCalendarOfItemsQuery : CalendarOfItemsCustomQuery
{
    public GetSubTasksNextTenItemsCalendarOfItemsQuery(DateOnly date, int pageNumber = 1)
        : base(
            CalendarOfItemsQueryTypes.SubTask,
            new DateOnlyRange(date.StartOfMonth(), date.EndOfMonth()),
            10,
            pageNumber < 0 ? 1 : pageNumber,
            CalendarOfItemsSortMode.NotSpecified,
            $"No required tasks for {date:MMMM yyyy}"
        ) { }
}
