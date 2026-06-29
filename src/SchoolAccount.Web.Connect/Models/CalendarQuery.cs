using SchoolAccount.Application.Features.CalendarOfItems.Common.Enums;

namespace SchoolAccount.Web.Connect.Models;

public class CalendarQuery
{
    public CalendarOfItemsViewModes ViewModes { get; init; } = CalendarOfItemsViewModes.None;

    public int PageSize { get; init; } = 20;

    public int PageNumber { get; init; } = 1;

    public CalendarOfItemsSortMode SortMode { get; init; } = CalendarOfItemsSortMode.NotSpecified;

    public Dictionary<string, List<string>> Filters { get; init; } = [];
}
