using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Features.CalendarOfItems.Contracts;
using SchoolAccount.Kernel.CalendarOfItems;

namespace SchoolAccount.Application.Features.CalendarOfItems.Query;

public record CalendarOfItemsDirectionalQuery(
    CalendarOfItemsQueryTypes ToQuery,
    CalendarOfItemsViewMode ViewMode,
    int ViewPeriodInMonths,
    DateOnly QueryFromDate,
    int PageSize,
    int PageNumber,
    CalendarOfItemsSortMode SortMode
) : IQuery<CalendarOfItemsPagedResult>;
