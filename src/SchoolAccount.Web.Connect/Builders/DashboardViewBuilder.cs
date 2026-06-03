using System.Collections.ObjectModel;
using SchoolAccount.Application.Features.CalendarOfItems.Common.Models;
using SchoolAccount.Application.Features.Category.Contracts;
using SchoolAccount.Application.Features.Category.Enums;
using SchoolAccount.Application.Features.Shared.Query.Contracts;
using SchoolAccount.Kernel;
using SchoolAccount.Web.Connect.Builders.CalendarOfItems;
using SchoolAccount.Web.Connect.Builders.Categories;
using SchoolAccount.Web.Connect.Features.CalendarOfItems;
using SchoolAccount.Web.Connect.Models;

namespace SchoolAccount.Web.Connect.Builders;

public class DashboardViewBuilder(
    CalendarOfItemsViewBuilder calendarOfItemsViewBuilder,
    CategoryListViewBuilder categoryListViewBuilder
)
{
    public DashboardViewModel Build(
        QueryPagedResult<CalendarOfItemsRow> queryPagedResult,
        CategoryPagedResult categoryPagedResult,
        Uri currentUri
    )
    {
        var dashboardViewItems = new Collection<DashboardViewItem>();

        dashboardViewItems.Add(
            new DashboardViewItem(
                CalendarOfItemsConstants.Views.Tab,
                calendarOfItemsViewBuilder.BuildForDashboard(queryPagedResult, currentUri)
            )
        );

        dashboardViewItems.Add(
            new DashboardViewItem(
                ViewAddressConstraints.Categories.List,
                categoryListViewBuilder.BuildForDashboard(
                    categoryPagedResult,
                    CategoryListViewModes.Dashboard,
                    currentUri
                )
            )
        );

        return new DashboardViewModel(Result.Success(), dashboardViewItems);
    }
}
