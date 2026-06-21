using SchoolAccount.Application.Features.Calendars.CalendarList.Contracts;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Abstractions.Aggregators;

public interface ICalendarOfItemsAggregator
{
    Task<Result<CalendarOfItemsPagedResult>> Query(
        CalendarOfItemsCriteria criteria,
        CancellationToken cancellationToken = default
    );
}
