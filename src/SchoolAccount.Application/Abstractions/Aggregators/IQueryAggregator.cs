using SchoolAccount.Application.Features.Shared.Query.Contracts;
using SchoolAccount.Application.Features.Shared.Query.Interfaces;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Abstractions.Aggregators;

public interface IQueryAggregator
{
    Task<Result<GenericQueryPagedResult<TRow>>> Query<TRow>(IEnumerable<IQueryFactory<TRow>> factories,
        GenericQueryCriteria<TRow> criteria, CancellationToken cancellationToken = default)
        where TRow: IQueryRow;
}
