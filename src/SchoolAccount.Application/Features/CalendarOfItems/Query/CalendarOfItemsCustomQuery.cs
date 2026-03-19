using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Features.CalendarOfItems.Contracts;
using SchoolAccount.Kernel;
using SchoolAccount.Kernel.CalendarOfItems;

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
