using SchoolAccount.Application.Features.Calendars.CalendarOfItems.Enums;
using SchoolAccount.Application.Features.Calendars.CalendarOfItems.Models;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Features.Calendars.CalendarOfItems.Contracts;

public class CalendarOfItemsCriteria
{
    public CalendarOfItemsQueryTypes ToQuery { get; init; }
    public int PageSize { get; init; }
    public int PageNumber { get; init; }
    public DateOnlyRange Range { get; init; }
    public CalendarOfItemsViewModes ViewModes { get; init; }
    public CalendarOfItemsSortMode SortMode { get; init; }
    public CalendarOfItemsFilter Filter { get; init; } = new([]);
    public bool IncludeFilterOptions { get; init; } = true;
    public CalendarOfItemsOrderFunction? CustomOrderByFunction { get; init; }
}
