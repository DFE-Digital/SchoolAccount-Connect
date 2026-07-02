using System.Collections.ObjectModel;
using SchoolAccount.Application.Extensions;
using SchoolAccount.Application.Features.Calendars.CalendarOfItems.Contracts;
using SchoolAccount.Application.Features.Calendars.CalendarOfItems.Enums;
using SchoolAccount.Application.Features.Calendars.CalendarOfItems.Query;
using SchoolAccount.Kernel;
using SchoolAccount.Web.Connect.Extensions;
using SchoolAccount.Web.Connect.Features.Shared.Filter;

namespace SchoolAccount.Web.Connect.Features.Calendars.CalendarOfItems;

public class CalendarOfItemsViewModelBuilder(IOrganisationContext organisationContext)
{
    private readonly CalendarOfItemsRowViewModelBuilder _rowViewModelBuilder = new();

    public CalendarOfItemsViewModel Build(
        CalendarOfItemsViewOptions options,
        CalendarOfItemsResponse result,
        Uri currentUri
    )
    {
        Collection<CalendarOfItemsRowGroupViewModel> rows = [];

        if (result.Payload.Items.Count > 0)
        {
            rows = result
                .Payload.Items.GroupBy(x =>
                    options.GroupingFunction is not null ? options.GroupingFunction(x) : string.Empty
                )
                .Select(x => new CalendarOfItemsRowGroupViewModel(
                    x.Key,
                    x.Select(r => _rowViewModelBuilder.Build(options, r))
                ))
                .ToCollection();
        }

        var pagedList = rows.ToStaticPagedList(
            result.Payload.PageNumber,
            result.Payload.PageSize,
            result.Payload.TotalCount
        );

        return new CalendarOfItemsViewModel(
            options.Title,
            options.Description,
            options.ViewMode,
            options.Tabs ?? [],
            pagedList,
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
        CalendarOfItemsResponse items,
        CalendarOfItemsViewModes viewModes,
        Uri currentUri
    )
    {
        CalendarOfItemsTabViewModel BuildTab(CalendarOfItemsViewModes mode, string label, string? description = null)
        {
            var key = nameof(CalendarOfItemsQuery.ViewModes);
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

        var lastUpdatedDate = items.Payload.Items.Select(x => x.LastUpdated).OfType<DateTime>().Cast<DateTime?>().Max();

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
