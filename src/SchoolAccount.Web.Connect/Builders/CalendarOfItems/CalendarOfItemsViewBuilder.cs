using System.Collections.ObjectModel;
using SchoolAccount.Application.Extensions;
using SchoolAccount.Application.Features.CalendarOfItems.Common.Enums;
using SchoolAccount.Application.Features.CalendarOfItems.Common.Models;
using SchoolAccount.Application.Features.Shared.Query.Contracts;
using SchoolAccount.Kernel;
using SchoolAccount.Web.Connect.Extensions;
using SchoolAccount.Web.Connect.Models.CalendarOfItems;
using SchoolAccount.Web.Connect.Models.Shared;

namespace SchoolAccount.Web.Connect.Builders.CalendarOfItems;

public class CalendarOfItemsViewBuilder(IOrganisationContext organisationContext)
{
    private readonly CalendarOfItemsRowViewBuilder _rowViewBuilder = new();
    private readonly PaginationViewBuilder _paginationViewBuilder = new();

    public CalendarOfItemsViewModel Build(
        CalendarOfItemViewOptions options,
        GenericQueryPagedResult<CalendarOfItemsRow> result,
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
        GenericQueryPagedResult<CalendarOfItemsRow> items,
        CalendarOfItemsViewModes viewModes,
        Uri currentUri
    )
    {
        CalendarOfItemsTabViewModel BuildTab(CalendarOfItemsViewModes mode, string label, string? description = null)
        {
            var value = mode.ToString();

            var updatedUrl = currentUri.SetQueryParam("ViewModes", value).RemoveQueryParam("pageNumber");

            return new CalendarOfItemsTabViewModel(label, description, updatedUrl, viewModes.HasFlag(mode));
        }

        var tabOptions = new[]
        {
            BuildTab(CalendarOfItemsViewModes.Forward, "Upcoming tasks"),
            BuildTab(CalendarOfItemsViewModes.Backward, "Previous tasks"),
        };

        var selectedTab = tabOptions.FirstOrDefault(x => x.IsSelected);

        var lastUpdatedDate = items.Payload.Select(x => x.LastUpdated).OfType<DateTime>().Cast<DateTime?>().Max();

        var options = new CalendarOfItemViewOptions
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

    public CalendarOfItemsViewModel BuildForDashboard(GenericQueryPagedResult<CalendarOfItemsRow> items, Uri currentUri)
    {
        var lastUpdatedDate = items.Payload.Select(x => x.LastUpdated).OfType<DateTime>().Cast<DateTime?>().Max();

        var options = new CalendarOfItemViewOptions
        {
            ViewMode = CalendarOfItemsViewModes.Custom | CalendarOfItemsViewModes.Standalone,
            Tabs = [],
            Title = "Upcoming tasks",
            Description = "These are the next required tasks that you must complete for your school.",
            GroupingFunction = x => x.SortDate?.ToString("MMMMM yyyy", null)!,
            NoResultsMessage = "No results found",
            LastUpdatedMessage = lastUpdatedDate is not null
                ? $"Last updated: {lastUpdatedDate.ToGdsDateString()}"
                : string.Empty,
        };

        return Build(options, items, currentUri);
    }
}
