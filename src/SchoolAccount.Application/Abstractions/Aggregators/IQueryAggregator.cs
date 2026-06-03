using SchoolAccount.Application.Features.Shared.Filtering.Interfaces;
using SchoolAccount.Application.Features.Shared.Query.Contracts;
using SchoolAccount.Application.Features.Shared.Query.Interfaces;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Abstractions.Aggregators;

public interface IQueryAggregator
{
    Task<Result<GenericQueryPagedResult<TRow>>> Query<TEntity, TRow>(
        IList<IQueryFactory<TEntity, TRow>> queryFactories,
        IList<IFilterableFactory> filterableFactories,
        GenericQueryCriteria<TRow> criteria,
        CancellationToken cancellationToken = default
    )
        where TEntity : IEntity
        where TRow : IQueryRow;
}
