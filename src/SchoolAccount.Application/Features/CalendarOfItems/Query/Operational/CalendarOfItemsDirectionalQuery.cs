using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Features.CalendarOfItems.Enums;
using SchoolAccount.Application.Features.CalendarOfItems.Models;
using SchoolAccount.Application.Features.Shared.Filtering.Models;
using SchoolAccount.Application.Features.Shared.Query.Contracts;
using SchoolAccount.Application.Features.Shared.Query.Delegates;

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
