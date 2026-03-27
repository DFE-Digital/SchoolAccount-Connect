using SchoolAccount.Application.Features.CalendarOfItems.Models;
using SchoolAccount.Application.Features.CalendarOfItems.Enums;

namespace SchoolAccount.Infrastructure.Abstraction;

public interface ICalendarOfItemsQueryFactory
{
    bool IsQueryableFor(CalendarOfItemsQueryTypes identifier);
    IQueryable<CalendarOfItemsRow> Query();
}
