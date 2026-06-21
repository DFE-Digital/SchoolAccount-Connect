using SchoolAccount.Application.Features.Calendars.CalendarList.Enums;
using SchoolAccount.Application.Features.Calendars.CalendarList.Models;
using SchoolAccount.Infrastructure.Helpers.Filtering;

namespace SchoolAccount.Infrastructure.Abstraction;

public interface ICalendarOfItemsQueryFactory
{
    bool IsQueryableFor(CalendarOfItemsQueryTypes identifier);
    IQueryable<CalendarOfItemsRow> Query(CalendarOfItemsFilter filter, FieldSelectorMapping mappings);
}
