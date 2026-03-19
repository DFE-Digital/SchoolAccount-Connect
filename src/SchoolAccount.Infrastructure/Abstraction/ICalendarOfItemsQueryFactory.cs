using SchoolAccount.Application.Features.CalendarOfItems.Models;
using SchoolAccount.Kernel.CalendarOfItems;

namespace SchoolAccount.Infrastructure.Abstraction;

public interface ICalendarOfItemsQueryFactory
{
    bool IsQueryableFor(CalendarOfItemsQueryTypes identifier);
    IQueryable<CalendarOfItemsRow> Query();
}
