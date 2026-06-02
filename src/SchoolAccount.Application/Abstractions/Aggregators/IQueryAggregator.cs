using SchoolAccount.Application.Features.CalendarOfItems.Contracts;
using SchoolAccount.Application.Features.CalendarOfItems.Models;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Abstractions.Aggregators;

public interface IQueryAggregator
{
    Task<Result<QueryPagedResult<TRow>>> Query<TRow>(IEnumerable<IQueryFactory<TRow>> factories,
        GenericQueryCriteria<TRow> criteria, CancellationToken cancellationToken = default)
        where TRow: IQueryRow;
}
