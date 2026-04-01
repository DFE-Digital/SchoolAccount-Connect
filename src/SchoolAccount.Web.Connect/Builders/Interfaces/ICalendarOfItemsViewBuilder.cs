using SchoolAccount.Application.Features.CalendarOfItems.Contracts;
using SchoolAccount.Application.Features.CalendarOfItems.Enums;
using SchoolAccount.Web.Connect.Models.CalendarOfItems;

namespace SchoolAccount.Web.Connect.Builders.Interfaces;

public interface ICalendarOfItemsViewBuilder
{
    CalendarOfItemsViewModel Build(CalendarOfItemViewOptions options, CalendarOfItemsPagedResult result);

    CalendarOfItemsViewModel BuildForPage(CalendarOfItemsPagedResult items, CalendarOfItemsViewModes viewModes);

    CalendarOfItemsViewModel BuildForDashboard(CalendarOfItemsPagedResult items);
}
