using System.Collections.ObjectModel;
using SchoolAccount.Application.Features.CalendarOfItems.Enums;
using SchoolAccount.Web.Connect.Models.CalendarOfItems;
using SchoolAccount.Web.Connect.Models.Shared;

namespace SchoolAccount.Web.Connect.Models.Categories;

public record CategoryHubViewModel(
    string? Title,
    string? Description,
    CalendarOfItemsViewModes ViewModes,
    Collection<CalendarOfItemsTabViewModel> Tabs,
    Collection<CalendarOfItemsRowGroupViewModel> Items,
    PaginationViewModel Pagination,
    FiltrationViewModel Filters
) : CalendarOfItemsViewModel(Title, Description, ViewModes, Tabs, Items, Pagination, Filters)
{
    public static CategoryHubViewModel FromCalendarOfItemsViewModel(CalendarOfItemsViewModel model)
    {
        return new CategoryHubViewModel(
            model.Title,
            model.Description,
            model.ViewModes,
            model.Tabs,
            model.Items,
            model.Pagination,
            model.Filters
        )
        {
            CallToActionMessage = model.CallToActionMessage,
            NoResultsMessage = model.NoResultsMessage,
            LastUpdatedMessage = model.LastUpdatedMessage,
            CanRenderFilter = model.CanRenderFilter,
            Caption = model.Caption,
            Heading = model.Heading,
            SubHeading = model.SubHeading,
            GeneratedAt = model.GeneratedAt,
        };
    }
}
