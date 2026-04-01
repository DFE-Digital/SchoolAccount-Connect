using SchoolAccount.Application.Features.CalendarOfItems.Contracts;
using SchoolAccount.Application.Features.CalendarOfItems.Enums;
using SchoolAccount.Application.Features.CalendarOfItems.Query;
using SchoolAccount.Web.Connect.Models.CalendarOfItems;

namespace SchoolAccount.Web.Connect.Builders.Interfaces;

public interface ICalendarOfItemsViewBuilder
{
    CalendarOfItemsViewModel Build(CalendarOfItemViewOptions options, CalendarOfItemsPagedResult result);

    CalendarOfItemsViewModel BuildForPage(
        CalendarOfItemsPagedResult items,
        CalendarOfItemsViewModes viewModes,
        CancellationToken cancellationToken
    );

    CalendarOfItemsViewModel BuildForDashboard(CalendarOfItemsPagedResult items, CancellationToken cancellationToken);
}
