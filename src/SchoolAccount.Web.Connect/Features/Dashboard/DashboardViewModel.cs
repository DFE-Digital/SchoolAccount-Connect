using SchoolAccount.Web.Connect.Features.Shared.List;

namespace SchoolAccount.Web.Connect.Features.Dashboard;

public sealed record DashboardViewModel
{
    public required IReadOnlyCollection<DashboardCalendarGroup> CalendarGroups { get; init; }

    public string? CalendarLastUpdatedMessage { get; init; }

    public bool HasCalendarItems => CalendarGroups.Count > 0;

    public bool HasCalendarLastUpdatedMessage => !string.IsNullOrEmpty(CalendarLastUpdatedMessage);

    public required IReadOnlyCollection<ListItemViewModel> Categories { get; init; }

    public bool NoCategoriesFound => Categories.Count == 0;

    public bool DisplayCategoryCallToAction => Categories.Count > 10;
}

public sealed record DashboardCalendarGroup(string MonthLabel, IReadOnlyCollection<DashboardCalendarItem> Items);

public sealed record DashboardCalendarItem(string Name, string Url)
{
    public string? Description { get; init; }

    public string? DateText { get; init; }
}
