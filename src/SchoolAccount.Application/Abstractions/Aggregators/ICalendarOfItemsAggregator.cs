using SchoolAccount.Application.Features.CalendarOfItems.Contracts;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Abstractions.Aggregators;

public interface ICalendarOfItemsAggregator
{
    Task<Result<CalendarOfItemsPagedResult>> Query<TFilter>(
        CalendarOfItemsCriteria criteria,
        CancellationToken cancellationToken = default
    ) where TFilter : IFilter;
}
