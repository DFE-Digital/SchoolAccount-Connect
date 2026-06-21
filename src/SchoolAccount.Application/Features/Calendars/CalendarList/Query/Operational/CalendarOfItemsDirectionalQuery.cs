using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Features.Calendars.CalendarList.Contracts;
using SchoolAccount.Application.Features.Calendars.CalendarList.Enums;
using SchoolAccount.Application.Features.Calendars.CalendarList.Models;

namespace SchoolAccount.Application.Features.Calendars.CalendarList.Query.Operational;

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
) : IQuery<CalendarOfItemsPagedResult>;
