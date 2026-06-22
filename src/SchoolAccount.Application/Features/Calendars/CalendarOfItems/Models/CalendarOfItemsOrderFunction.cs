namespace SchoolAccount.Application.Features.Calendars.CalendarOfItems.Models;

public delegate IOrderedQueryable<CalendarOfItemsRow> CalendarOfItemsOrderFunction(
    IQueryable<CalendarOfItemsRow> query
);
