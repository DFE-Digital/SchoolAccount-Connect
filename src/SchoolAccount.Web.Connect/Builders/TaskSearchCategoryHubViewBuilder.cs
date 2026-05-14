using System.Collections.ObjectModel;
using SchoolAccount.Application.Features.CalendarOfItems.Enums;
using SchoolAccount.Application.Features.Tasks.Search.Queries.GetPage;
using SchoolAccount.Web.Connect.Models.CalendarOfItems;
using SchoolAccount.Web.Connect.Models.Categories;
using SchoolAccount.Web.Connect.Models.Shared;

namespace SchoolAccount.Web.Connect.Builders;

public class TaskSearchCategoryHubViewBuilder
{
    public CategoryHubViewModel Build(TaskSearchResponse searchResults, string? term, Uri currentUri)
    {
        var items = new Collection<CalendarOfItemsRowGroupViewModel>
        {
            new(
                string.Empty,
                searchResults.Tasks.Select(task => new CalendarOfItemsRowItemViewModel(
                    task.Name,
                    string.Format(Thread.CurrentThread.CurrentCulture, RouteConstants.Task.Index, task.Id)
                )
                {
                    Description = task.Description,
                    DateText = $"Last updated {task.DateUpdated:d MMMM yyyy}.",
                })
            ),
        };

        return new CategoryHubViewModel(
            Title: "Search results",
            Description: string.IsNullOrWhiteSpace(term)
                ? "Showing matching tasks."
                : $"Showing results for “{term}”.",
            ViewModes: CalendarOfItemsViewModes.Custom,
            Tabs: [],
            Items: items,
            Pagination: new PaginationViewModel(false),
            Filters: FiltrationViewModel.Build(CalendarOfItemsViewModes.Custom, currentUri, [])
        )
        {
            Heading = "Search results",
            SubHeading = searchResults.Tasks.Count == 0
                ? "No matching tasks found."
                : $"{searchResults.Tasks.Count} task{(searchResults.Tasks.Count == 1 ? string.Empty : "s")} found.",
            NoResultsMessage = "No matching tasks found.",
            CanRenderFilter = false,
            LastUpdatedMessage = "Last updated: today",
        };
    }
}