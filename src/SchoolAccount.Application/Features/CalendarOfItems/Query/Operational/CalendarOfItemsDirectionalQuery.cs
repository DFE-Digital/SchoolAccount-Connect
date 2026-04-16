using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Features.CalendarOfItems.Contracts;
using SchoolAccount.Application.Features.CalendarOfItems.Enums;
using SchoolAccount.Application.Features.CalendarOfItems.Models;

namespace SchoolAccount.Application.Features.CalendarOfItems.Query.Operational;

public abstract record CalendarOfItemsDirectionalQuery(
    CalendarOfItemsQueryTypes ToQuery,
    CalendarOfItemsViewModes ViewModes,
    int ViewPeriodInMonths,
    DateOnly QueryFromDate,
    int PageSize,
    int PageNumber,
    CalendarOfItemsSortMode SortMode,
    CalendarOfItemsFilter? Filter = null
) : IQuery<CalendarOfItemsPagedResult>;
