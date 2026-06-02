namespace SchoolAccount.Application.Features.CalendarOfItems.Models;

public delegate IOrderedQueryable<T> GenericOrderFunction<T>(
    IQueryable<T> query
) where T : IQueryRow;