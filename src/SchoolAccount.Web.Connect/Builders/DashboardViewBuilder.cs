using System.Collections.ObjectModel;
using SchoolAccount.Application.Features.CalendarOfItems.Contracts;
using SchoolAccount.Application.Features.Category.Contracts;
using SchoolAccount.Application.Features.Category.Enums;
using SchoolAccount.Kernel;
using SchoolAccount.Web.Connect.Builders.CalendarOfItems;
using SchoolAccount.Web.Connect.Builders.Categories;
using SchoolAccount.Web.Connect.Models;

namespace SchoolAccount.Web.Connect.Builders;

public class DashboardViewBuilder
{
    public DashboardViewModel Build(
        CalendarOfItemsPagedResult calendarOfItemsPagedResult,
        CategoryPagedResult categoryPagedResult,
        IOrganisationContext organisationContext,
        Uri currentUri
    )
    {
        var calendarOfItemsViewBuilder = new CalendarOfItemsViewBuilder(organisationContext);
        var categoryListViewBuilder = new CategoryListViewBuilder();
        var dashboardViewItems = new Collection<DashboardViewItem>();

        dashboardViewItems.Add(
            new DashboardViewItem(
                ViewAddressConstraints.CalendarOfItems.Tab,
                calendarOfItemsViewBuilder.BuildForDashboard(calendarOfItemsPagedResult, currentUri)
            )
        );

        dashboardViewItems.Add(
            new DashboardViewItem(
                ViewAddressConstraints.Categories.List,
                categoryListViewBuilder.BuildForPage(categoryPagedResult, CategoryListViewModes.None, currentUri)
            )
        );

        return new DashboardViewModel(Result.Success(), dashboardViewItems);
    }
}
