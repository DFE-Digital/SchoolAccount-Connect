namespace SchoolAccount.Application.Features.CalendarOfItems.Models;

public delegate IOrderedQueryable<CalendarOfItemsRow> CalendarOfItemsOrderFunction(
    IQueryable<CalendarOfItemsRow> query
);
