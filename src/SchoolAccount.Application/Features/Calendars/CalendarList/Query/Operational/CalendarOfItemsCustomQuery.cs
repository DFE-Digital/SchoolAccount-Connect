using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Features.Calendars.CalendarList.Contracts;
using SchoolAccount.Application.Features.Calendars.CalendarList.Enums;
using SchoolAccount.Application.Features.Calendars.CalendarList.Models;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Features.Calendars.CalendarList.Query.Operational;

public abstract record CalendarOfItemsCustomQuery(
    CalendarOfItemsQueryTypes ToQuery,
    DateOnlyRange QueryRange,
    int PageSize,
    int PageNumber,
    CalendarOfItemsSortMode SortMode,
    string NoResultMessage,
    CalendarOfItemsFilter? Filter = null,
    CalendarOfItemsOrderFunction? CustomOrderBy = null
) : IQuery<CalendarOfItemsPagedResult>;
