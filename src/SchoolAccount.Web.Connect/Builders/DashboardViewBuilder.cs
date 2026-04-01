using System.Collections.ObjectModel;
using SchoolAccount.Application.Features.CalendarOfItems.Contracts;
using SchoolAccount.Kernel;
using SchoolAccount.Web.Connect.Builders.Interfaces;
using SchoolAccount.Web.Connect.Models;

namespace SchoolAccount.Web.Connect.Builders;

public class DashboardViewBuilder(ICalendarOfItemsViewBuilder calendarOfItemsViewBuilder) : IDashboardViewBuilder
{
    public DashboardViewModel Build(CalendarOfItemsPagedResult items, CancellationToken cancellationToken)
    {
        var dashboardViewItems = new Collection<DashboardViewItem>();

        dashboardViewItems.Add(
            new DashboardViewItem(
                ViewAddressConstraints.CalendarOfItems.Tab,
                calendarOfItemsViewBuilder.BuildForDashboard(items, cancellationToken)
            )
        );

        return new DashboardViewModel(Result.Success(), dashboardViewItems);
    }
}
