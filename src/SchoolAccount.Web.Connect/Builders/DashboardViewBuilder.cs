using System.Collections.ObjectModel;
using SchoolAccount.Application.Features.CalendarOfItems.Contracts;
using SchoolAccount.Kernel;
using SchoolAccount.Web.Connect.Models;

namespace SchoolAccount.Web.Connect.Builders;

public class DashboardViewBuilder
{
    public DashboardViewModel Build(
        CalendarOfItemsPagedResult items,
        IOrganisationContext organisationContext,
        Uri currentUri
    )
    {
        var calendarOfItemsViewBuilder = new CalendarOfItemsViewBuilder(organisationContext);
        var dashboardViewItems = new Collection<DashboardViewItem>();

        dashboardViewItems.Add(
            new DashboardViewItem(
                ViewAddressConstraints.CalendarOfItems.Tab,
                calendarOfItemsViewBuilder.BuildForDashboard(items, currentUri)
            )
        );

        return new DashboardViewModel(Result.Success(), dashboardViewItems);
    }
}
