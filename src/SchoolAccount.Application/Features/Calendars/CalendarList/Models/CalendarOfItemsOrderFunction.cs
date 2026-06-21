namespace SchoolAccount.Application.Features.Calendars.CalendarList.Models;

public delegate IOrderedQueryable<CalendarOfItemsRow> CalendarOfItemsOrderFunction(
    IQueryable<CalendarOfItemsRow> query
);
