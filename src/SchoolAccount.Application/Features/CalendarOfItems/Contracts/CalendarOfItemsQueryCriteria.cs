using SchoolAccount.Application.Features.CalendarOfItems.Enums;
using SchoolAccount.Application.Features.CalendarOfItems.Models;
using SchoolAccount.Application.Features.Shared.Query.Contracts;

namespace SchoolAccount.Application.Features.CalendarOfItems.Contracts;

public class CalendarOfItemsQueryCriteria : GenericQueryCriteria<CalendarOfItemsRow>
{
    public CalendarOfItemsViewModes ViewModes { get; init; }
    public CalendarOfItemsSortMode SortMode { get; init; }
}