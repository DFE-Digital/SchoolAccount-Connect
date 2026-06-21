using System.Collections.ObjectModel;
using SchoolAccount.Application.Extensions;
using SchoolAccount.Application.Features.Calendars.CalendarOfItems.Contracts;
using SchoolAccount.Application.Features.Calendars.CalendarOfItems.Enums;
using SchoolAccount.Application.Features.Calendars.CalendarOfItems.Query.Operational;
using SchoolAccount.Kernel;
using SchoolAccount.Web.Connect.Builders;
using SchoolAccount.Web.Connect.Extensions;
using SchoolAccount.Web.Connect.Models.Shared;

namespace SchoolAccount.Web.Connect.Features.Calendars.CalendarOfItems;

public class CalendarOfItemsViewModelBuilder(IOrganisationContext organisationContext)
{
    private readonly CalendarOfItemsRowViewBuilder _rowViewBuilder = new();
    private readonly PaginationViewBuilder _paginationViewBuilder = new();

    public CalendarOfItemsViewModel Build(
        CalendarOfItemsViewOptions options,
        CalendarOfItemsPagedResult result,
        Uri currentUri
    )
    {
        Collection<CalendarOfItemsRowGroupViewModel> rows = [];

        if (result.Payload.Count > 0)
        {
            rows = result
                .Payload.GroupBy(x => options.GroupingFunction is not null ? options.GroupingFunction(x) : string.Empty)
                .Select(x => new CalendarOfItemsRowGroupViewModel(
                    x.Key,
                    x.Select(r => _rowViewBuilder.Build(options, r))
                ))
                .ToCollection();
        }

        return new CalendarOfItemsViewModel(
            options.Title,
            options.Description,
            options.ViewMode,
            options.Tabs ?? [],
            rows,
            !options.ViewMode.HasFlag(CalendarOfItemsViewModes.Standalone)
                ? _paginationViewBuilder.Build(result, currentUri)
                : new PaginationViewModel(false),
            FiltrationViewModel.Build(options.ViewMode, currentUri, result.Filter)
        )
        {
            GeneratedAt = result.GeneratedDate,
            LastUpdatedMessage = options.LastUpdatedMessage,
            CallToActionMessage = options.CallToActionMessage,
            NoResultsMessage = options.NoResultsMessage,
            Heading = options.Heading,
            SubHeading = options.SubHeading,
            Title = options.Title,
            Description = options.Description,
            Caption = organisationContext.Organisation.Name,
        };
    }

    public CalendarOfItemsViewModel BuildForPage(
        CalendarOfItemsPagedResult items,
        CalendarOfItemsViewModes viewModes,
        Uri currentUri
    )
    {
        CalendarOfItemsTabViewModel BuildTab(CalendarOfItemsViewModes mode, string label, string? description = null)
        {
            var key = nameof(CalendarOfItemsDirectionalQuery.ViewModes);
            var value = mode.ToString();

            var updatedUrl = currentUri.SetQueryParam(key, value).RemoveQueryParam("pageNumber");

            return new CalendarOfItemsTabViewModel(label, description, updatedUrl, viewModes.HasFlag(mode));
        }

        var tabOptions = new[]
        {
            BuildTab(CalendarOfItemsViewModes.Forward, "Upcoming tasks"),
            BuildTab(CalendarOfItemsViewModes.Backward, "Previous tasks"),
        };

        var selectedTab = tabOptions.FirstOrDefault(x => x.IsSelected);

        var lastUpdatedDate = items.Payload.Select(x => x.LastUpdated).OfType<DateTime>().Cast<DateTime?>().Max();

        var options = new CalendarOfItemsViewOptions
        {
            ViewMode = viewModes,
            Tabs = tabOptions.ToCollection(),
            Title = selectedTab?.Label ?? string.Empty,
            Description = selectedTab?.Description ?? string.Empty,
            Heading = "Calendar of tasks",
            SubHeading = "These are all of the required tasks that you must complete for your school.",
            GroupingFunction = x => x.SortDate.ToGdsMonthString(),
            NoResultsMessage = "No results found",
            LastUpdatedMessage = lastUpdatedDate is not null
                ? $"Last updated: {lastUpdatedDate.ToGdsDateString()}"
                : string.Empty,
        };

        return Build(options, items, currentUri);
    }
}
