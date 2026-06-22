using SchoolAccount.Application.Features.Calendars.CalendarOfItems.Contracts;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Abstractions.Aggregators;

public interface ICalendarOfItemsAggregator
{
    Task<Result<CalendarOfItemsResponse>> Query(
        CalendarOfItemsCriteria criteria,
        CancellationToken cancellationToken = default
    );
}
