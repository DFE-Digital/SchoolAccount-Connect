using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Features.CalendarOfItems.Contracts;
using SchoolAccount.Application.Features.CalendarOfItems.Enums;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Features.CalendarOfItems.Query;

public record CalendarOfItemsCustomQuery(
    CalendarOfItemsQueryTypes ToQuery,
    CalendarOfItemsViewMode ViewMode,
    int ViewPeriodInMonths,
    DateOnlyRange QueryRange,
    int PageSize,
    int PageNumber,
    CalendarOfItemsSortMode SortMode
) : IQuery<CalendarOfItemsPagedResult>;
