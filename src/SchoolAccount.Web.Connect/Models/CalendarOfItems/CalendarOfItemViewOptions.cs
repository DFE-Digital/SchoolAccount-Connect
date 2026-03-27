using System.Collections.ObjectModel;
using SchoolAccount.Application.Features.CalendarOfItems.Enums;
using SchoolAccount.Application.Features.CalendarOfItems.Models;

namespace SchoolAccount.Web.Connect.Models.CalendarOfItems;

public class CalendarOfItemViewOptions
{
    public string? BaseUri { get; set; }
    public CalendarOfItemsViewModes ViewMode { get; set; }
    public Collection<CalendarOfItemsTabViewModel>? Tabs { get; init; }
    public string? CallToActionMessage { get; init; }
    public string? NoResultsMessage { get; init; }
    public Func<CalendarOfItemsRow, string>? GroupingFunction { get; init; }
    public string? Heading { get; init; }
    public string? SubHeading { get; init; }
    public string Title { get; init; } = null!;
    public string Description { get; init; } = null!;
    public string? LastUpdatedMessage { get; init; }
}
