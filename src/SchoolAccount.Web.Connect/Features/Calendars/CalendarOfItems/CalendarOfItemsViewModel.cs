using System.Collections.ObjectModel;
using SchoolAccount.Application.Extensions;
using SchoolAccount.Application.Features.Calendars.CalendarOfItems.Enums;
using SchoolAccount.Web.Connect.Models.Shared;

namespace SchoolAccount.Web.Connect.Features.Calendars.CalendarOfItems;

public class CalendarOfItemsRowGroupViewModel(string value, IEnumerable<CalendarOfItemsRowViewModel> items)
    : Collection<CalendarOfItemsRowViewModel>(items.ToCollection())
{
    public string? DisplayValue { get; } = value;
    public bool HasDisplayValue => !string.IsNullOrEmpty(DisplayValue);
}

public record CalendarOfItemsViewModel(
    string? Title,
    string? Description,
    CalendarOfItemsViewModes ViewModes,
    Collection<CalendarOfItemsTabViewModel> Tabs,
    Collection<CalendarOfItemsRowGroupViewModel> Items,
    PaginationViewModel Pagination,
    FiltrationViewModel Filters
)
{
    private readonly string _callToActionMessage = "See the full calendar of tasks";

    public bool IsStandalone => ViewModes.HasFlag(CalendarOfItemsViewModes.Standalone);

    public DateTime GeneratedAt { get; init; } = DateTime.UtcNow;

    public string? Caption { get; init; }
    public bool HasCaption => !string.IsNullOrEmpty(Caption);
    public string? Heading { get; init; }
    public bool HasHeading => !string.IsNullOrEmpty(Heading);
    public string? SubHeading { get; init; }
    public bool HasSubHeading => !string.IsNullOrEmpty(SubHeading);
    public bool ShowPageHeading => HasCaption || HasHeading || HasSubHeading;

    public string DisplayTitle =>
        Title ?? SelectedTab?.Label ?? throw new InvalidOperationException("Display title cannot be determined");

    public bool HasTabs => Tabs.Count > 0;
    public CalendarOfItemsTabViewModel? SelectedTab => Tabs.FirstOrDefault(x => x.IsSelected);
    public bool HasSelectedTab => SelectedTab is not null;
    public string CurrentlyActiveTabAccessibilityLabel => $"Currently active tab is {SelectedTab?.Label}";

    public string? CallToActionMessage
    {
        get => _callToActionMessage;
        init
        {
            if (!string.IsNullOrEmpty(value))
            {
                _callToActionMessage = value;
            }
        }
    }

    public string? NoResultsMessage { get; init; }
    public bool HasNoResultsMessage => !string.IsNullOrEmpty(NoResultsMessage);

    public string? LastUpdatedMessage { get; init; }
    public bool HasLastUpdatedMessage => !string.IsNullOrEmpty(LastUpdatedMessage);
    public bool HasTitle => !string.IsNullOrEmpty(Title);
    public bool HasDescription => !string.IsNullOrEmpty(Description);
    public bool ShowNavigator => ViewModes.HasFlag(CalendarOfItemsViewModes.Standalone);

    public bool CanRenderFilter { get; init; } = true;
    public bool HasFilters => CanRenderFilter && Filters.Count > 0;
}
