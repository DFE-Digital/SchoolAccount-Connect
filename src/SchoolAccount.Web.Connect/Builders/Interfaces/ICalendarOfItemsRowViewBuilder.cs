using SchoolAccount.Application.Features.CalendarOfItems.Enums;
using SchoolAccount.Application.Features.CalendarOfItems.Models;
using SchoolAccount.Web.Connect.Models.CalendarOfItems;

namespace SchoolAccount.Web.Connect.Builders.Interfaces;

public interface ICalendarOfItemsRowViewBuilder
{
    CalendarOfItemsRowItemViewModel Build(CalendarOfItemsViewModes mode, CalendarOfItemsRow row);
}
