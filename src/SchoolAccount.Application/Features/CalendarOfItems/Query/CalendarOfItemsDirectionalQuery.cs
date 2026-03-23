using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Features.CalendarOfItems.Contracts;
using SchoolAccount.Application.Features.CalendarOfItems.Enums;

namespace SchoolAccount.Application.Features.CalendarOfItems.Query;

public record CalendarOfItemsDirectionalQuery(
    CalendarOfItemsQueryTypes ToQuery,
    CalendarOfItemsViewModes ViewModes,
    int ViewPeriodInMonths,
    DateOnly QueryFromDate,
    int PageSize,
    int PageNumber,
    CalendarOfItemsSortMode SortMode
) : IQuery<CalendarOfItemsPagedResult>;
