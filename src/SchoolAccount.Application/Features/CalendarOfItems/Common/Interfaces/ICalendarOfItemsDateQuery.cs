using SchoolAccount.Application.Features.CalendarOfItems.Common.Enums;

namespace SchoolAccount.Application.Features.CalendarOfItems.Common.Interfaces;

public interface ICalendarOfItemsDateQuery
{
    CalendarOfItemsViewModes ViewModes { get; init; }
    DateOnly QueryFromDate { get; }
    int ViewPeriodInMonths { get; }
}