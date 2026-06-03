using SchoolAccount.Application.Features.CalendarOfItems.Common.Enums;
using SchoolAccount.Application.Features.CalendarOfItems.Common.Models;
using SchoolAccount.Application.Features.Category.Models;
using SchoolAccount.Application.Features.Shared.Query.Contracts;
using SchoolAccount.Web.Connect.Builders.CalendarOfItems;
using SchoolAccount.Web.Connect.Models.CalendarOfItems;
using SchoolAccount.Web.Connect.Models.Categories;

namespace SchoolAccount.Web.Connect.Builders.Categories;

public class CategoryHubViewBuilder(CalendarOfItemsViewBuilder calendarOfItemsViewBuilder)
{
    public CategoryHubViewModel Build(
        GenericQueryPagedResult<CalendarOfItemsRow> items,
        Uri currentUri,
        CategoryType? category = null
    )
    {
        var options = new CalendarOfItemViewOptions
        {
            ViewMode = CalendarOfItemsViewModes.Custom,
            Tabs = [],
            Description = $"Explore all tasks and support",
            Heading = category is not null ? category.DisplayName : "All tasks",
            SubHeading = category is not null
                ? category.HubViewDescription
                : "See all your tasks, returns and policies from DfE.",
            LastUpdatedMessage = "Last updated: today",
            NoResultsMessage = "No results found",
            CanRenderFilter = false,
        };

        return CategoryHubViewModel.FromCalendarOfItemsViewModel(
            calendarOfItemsViewBuilder.Build(options, items, currentUri),
            category?.Id
        );
    }
}
