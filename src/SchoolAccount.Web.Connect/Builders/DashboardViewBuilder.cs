using System.Collections.ObjectModel;
using SchoolAccount.Application.Extensions;
using SchoolAccount.Application.Features.CalendarOfItems.Enums;
using SchoolAccount.Application.Features.CalendarOfItems.Query;
using SchoolAccount.Kernel;
using SchoolAccount.Web.Connect.Builders.Interfaces;
using SchoolAccount.Web.Connect.Models;

namespace SchoolAccount.Web.Connect.Builders;

public class DashboardViewBuilder(ICalendarOfItemsViewBuilder calendarOfItemsViewBuilder) : IDashboardViewBuilder
{
    public async Task<DashboardViewModel> Build(CancellationToken cancellationToken)
    {
        var items = new Collection<DashboardViewItem>();
        var date = DateTime.Today;

        var calendarOfItemOptions = new CalendarOfItemsCustomQuery(
            CalendarOfItemsQueryTypes.SubTask,
            new DateOnlyRange(date.StartOfMonth().ToDateOnly(), date.EndOfMonth().ToDateOnly()),
            10,
            1,
            CalendarOfItemsSortMode.NotSpecified,
            $"No required tasks for {date:MMMM yyyy}"
        );
        items.Add(
            new DashboardViewItem(
                ViewAddressConstraints.CalendarOfItems.Tab,
                await calendarOfItemsViewBuilder.BuildForDashboard(calendarOfItemOptions, cancellationToken)
            )
        );

        return new DashboardViewModel(Result.Success(), items);
    }
}
