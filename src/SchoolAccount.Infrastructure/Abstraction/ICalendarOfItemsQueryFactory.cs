using SchoolAccount.Application.Features.CalendarOfItems.Enums;
using SchoolAccount.Application.Features.CalendarOfItems.Models;

namespace SchoolAccount.Infrastructure.Abstraction;

public interface ICalendarOfItemsQueryFactory
{
    bool IsQueryableFor(CalendarOfItemsQueryTypes identifier);
    IQueryable<CalendarOfItemsRow> Query();
}
