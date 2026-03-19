using SchoolAccount.Application.Features.CalendarOfItems.Contracts;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Abstractions.Infrastructure;

public interface ICalendarOfItemsAggregator
{
    Task<Result<CalendarOfItemsPagedResult>> Query(
        CalendarOfItemsCriteria criteria,
        CancellationToken cancellationToken = default
    );
}
