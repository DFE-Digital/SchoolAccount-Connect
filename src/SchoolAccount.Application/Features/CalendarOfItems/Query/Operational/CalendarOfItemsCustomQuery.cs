using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Features.CalendarOfItems.Contracts;
using SchoolAccount.Application.Features.CalendarOfItems.Enums;
using SchoolAccount.Application.Features.CalendarOfItems.Models;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Features.CalendarOfItems.Query.Operational;

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
