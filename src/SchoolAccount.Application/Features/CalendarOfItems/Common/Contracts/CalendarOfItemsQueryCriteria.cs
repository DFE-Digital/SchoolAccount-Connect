using SchoolAccount.Application.Features.CalendarOfItems.Common.Enums;
using SchoolAccount.Application.Features.CalendarOfItems.Common.Models;
using SchoolAccount.Application.Features.Shared.Query.Contracts;

namespace SchoolAccount.Application.Features.CalendarOfItems.Common.Contracts;

public class CalendarOfItemsQueryCriteria : GenericQueryCriteria<CalendarOfItemsRow>
{
    public CalendarOfItemsViewModes ViewModes { get; init; }
    public CalendarOfItemsSortMode SortMode { get; init; }
}