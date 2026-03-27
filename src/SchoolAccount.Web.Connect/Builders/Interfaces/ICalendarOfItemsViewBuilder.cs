using SchoolAccount.Application.Features.CalendarOfItems.Contracts;
using SchoolAccount.Application.Features.CalendarOfItems.Query;
using SchoolAccount.Web.Connect.Models.CalendarOfItems;

namespace SchoolAccount.Web.Connect.Builders.Interfaces;

public interface ICalendarOfItemsViewBuilder
{
    CalendarOfItemsViewModel Build(CalendarOfItemViewOptions options, CalendarOfItemsPagedResult result);

    Task<CalendarOfItemsViewModel> BuildForPage(
        CalendarOfItemsDirectionalQuery query,
        CancellationToken cancellationToken
    );

    Task<CalendarOfItemsViewModel> BuildForDashboard(
        CalendarOfItemsCustomQuery query,
        CancellationToken cancellationToken
    );
}
