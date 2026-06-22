using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Features.Calendars.CalendarOfItems.Contracts;
using SchoolAccount.Application.Features.Calendars.CalendarOfItems.Enums;
using SchoolAccount.Application.Features.Calendars.CalendarOfItems.Models;

namespace SchoolAccount.Application.Features.Calendars.CalendarOfItems.Query.Operational;

public abstract record CalendarOfItemsDirectionalQuery(
    CalendarOfItemsQueryTypes ToQuery,
    CalendarOfItemsViewModes ViewModes,
    int ViewPeriodInMonths,
    DateOnly QueryFromDate,
    int PageSize,
    int PageNumber,
    CalendarOfItemsSortMode SortMode,
    CalendarOfItemsFilter? Filter = null,
    CalendarOfItemsOrderFunction? CustomOrderBy = null
) : IQuery<CalendarOfItemsResponse>;
