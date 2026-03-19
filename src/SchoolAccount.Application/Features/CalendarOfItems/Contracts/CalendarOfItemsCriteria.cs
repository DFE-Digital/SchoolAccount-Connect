using SchoolAccount.Kernel;
using SchoolAccount.Kernel.CalendarOfItems;

namespace SchoolAccount.Application.Features.CalendarOfItems.Contracts;

public class CalendarOfItemsCriteria
{
    public CalendarOfItemsQueryTypes ToQuery { get; init; }
    public int PageSize { get; init; }
    public int PageNumber { get; init; }
    public DateOnlyRange Range { get; init; }
    public CalendarOfItemsViewMode ViewMode { get; init; }
    public CalendarOfItemsSortMode SortMode { get; init; }
}
