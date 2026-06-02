using SchoolAccount.Application.Abstractions;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Features.CalendarOfItems.Contracts;
using SchoolAccount.Application.Features.CalendarOfItems.Enums;
using SchoolAccount.Application.Features.CalendarOfItems.Models;
using SchoolAccount.Application.Features.Shared.Filtering;
using SchoolAccount.Application.Features.Shared.Filtering.Models;

namespace SchoolAccount.Application.Features.CalendarOfItems.Query.Operational;

public abstract record CalendarOfItemsDirectionalQuery(
    CalendarOfItemsViewModes ViewModes,
    int ViewPeriodInMonths,
    DateOnly QueryFromDate,
    int PageSize,
    int PageNumber,
    CalendarOfItemsSortMode SortMode,
    IList<FilterRequest>? Filter = null,
    GenericOrderFunction<CalendarOfItemsRow>? CustomOrderBy = null
) : IQuery<QueryPagedResult<CalendarOfItemsRow>>;
