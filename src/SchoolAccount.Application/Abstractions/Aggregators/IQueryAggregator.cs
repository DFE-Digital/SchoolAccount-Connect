using SchoolAccount.Application.Abstractions.Pipelines;
using SchoolAccount.Application.Features.Shared.Filtering.Interfaces;
using SchoolAccount.Application.Features.Shared.Query.Contracts;
using SchoolAccount.Application.Features.Shared.Query.Interfaces;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Abstractions.Aggregators;

public interface IQueryAggregator
{
    Task<Result<GenericQueryPagedResult<TRow>>> Query<TRow>(
        IQueryFactoryPipeline<TRow> factoryPipeline,
        IFilterablePipeline filterPipeline,
        GenericQueryCriteria<TRow> criteria,
        CancellationToken cancellationToken = default
    )
        where TRow : IQueryRow;
}
