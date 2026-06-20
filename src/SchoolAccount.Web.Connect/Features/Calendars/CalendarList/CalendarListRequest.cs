using SchoolAccount.Application.Features.CalendarOfItems.Enums;

namespace SchoolAccount.Web.Connect.Features.Calendar.CalendarList;

public class CalendarListRequest
{
    public CalendarOfItemsViewModes ViewModes { get; init; } = CalendarOfItemsViewModes.None;

    public int PageSize { get; init; } = 10;

    public int PageNumber { get; init; } = 1;

    public CalendarOfItemsSortMode SortMode { get; init; } = CalendarOfItemsSortMode.NotSpecified;

    public Dictionary<string, List<string>> Filters { get; init; } = [];
}
