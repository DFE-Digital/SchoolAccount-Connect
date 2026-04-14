using SchoolAccount.Application.Features.CalendarOfItems.Contracts;
using SchoolAccount.Application.Features.CalendarOfItems.Enums;
using SchoolAccount.Application.Features.Category.Models;
using SchoolAccount.Kernel;
using SchoolAccount.Web.Connect.Builders.CalendarOfItems;
using SchoolAccount.Web.Connect.Models.CalendarOfItems;

namespace SchoolAccount.Web.Connect.Builders.Categories;

public class CategoryHubViewBuilder(IOrganisationContext organisationContext)
{
    public CalendarOfItemsViewModel Build(
        CalendarOfItemsPagedResult items,
        Uri currentUri,
        CategoryType? category = null
    )
    {
        var options = new CalendarOfItemViewOptions
        {
            ViewMode = CalendarOfItemsViewModes.Custom | CalendarOfItemsViewModes.Standalone,
            Tabs = [],
            Description = $"Explore all tasks and support",
            Heading = category is not null ? category.DisplayName : "All tasks",
            SubHeading = category is not null ? category.HubViewDescription : "See all your tasks, returns and policies from DfE.",
            LastUpdatedMessage = "Last updated: today",
            NoResultsMessage = "No results found",
            CanRenderFilter = false
        };

        var calendarOfItemsViewBuilder = new CalendarOfItemsViewBuilder(organisationContext);

        return calendarOfItemsViewBuilder.Build(options, items, currentUri);
    }
}