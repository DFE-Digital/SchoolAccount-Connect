using SchoolAccount.Application.Features.CalendarOfItems.Contracts;
using SchoolAccount.Web.Connect.Models;

namespace SchoolAccount.Web.Connect.Builders.Interfaces;

public interface IDashboardViewBuilder
{
    DashboardViewModel Build(CalendarOfItemsPagedResult items, CancellationToken cancellationToken);
}
