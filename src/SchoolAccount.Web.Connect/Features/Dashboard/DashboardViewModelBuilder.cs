using SchoolAccount.Application.Extensions;
using SchoolAccount.Application.Features.Dashboard;
using SchoolAccount.Web.Connect.Features.Shared.ListItem;
using static SchoolAccount.Web.Connect.RouteConstants;

namespace SchoolAccount.Web.Connect.Features.Dashboard;

public sealed class DashboardViewModelBuilder
{
    public static DashboardViewModel Build(GetDashboardResponse response)
    {
        return new DashboardViewModel
        {
            CalendarGroups = BuildCalendarGroups(response.CalendarOfItems),
            CalendarLastUpdatedMessage = BuildLastUpdatedMessage(response.CalendarOfItems),
            Categories = BuildCategories(response.Categories),
        };
    }

    private static List<DashboardCalendarGroup> BuildCalendarGroups(
        IReadOnlyList<GetDashboardResponseCalendarItem> items
    ) =>
        items
            .GroupBy(x => x.SortDate?.ToString("MMMMM yyyy", null) ?? string.Empty)
            .Select(g => new DashboardCalendarGroup(g.Key, g.Select(MapCalendarItem).ToList()))
            .ToList();

    private static string? BuildLastUpdatedMessage(IReadOnlyList<GetDashboardResponseCalendarItem> items)
    {
        var lastUpdatedDate = items.Select(x => x.LastUpdated).OfType<DateTime>().Cast<DateTime?>().Max();
        return lastUpdatedDate is not null ? $"Last updated: {lastUpdatedDate.ToGdsDateString()}" : null;
    }

    private static DashboardCalendarItem MapCalendarItem(GetDashboardResponseCalendarItem item) =>
        new(item.Name, Url(RouteConstants.Task.Index, item.Id))
        {
            Description = item.Description,
            DateText = item.DateText,
        };

    private static List<ListItemViewModel> BuildCategories(IReadOnlyList<GetDashboardResponseCategoryItem> categories)
    {
        return categories
            .Select(c => new ListItemViewModel(c.DisplayName, Url(Category.Index, c.Id), description: c.Description))
            .ToList();
    }
}
