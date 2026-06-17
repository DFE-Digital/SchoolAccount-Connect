using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using SchoolAccount.Application.Abstractions.Pipelines;
using SchoolAccount.Application.Features.Shared.Query.Interfaces;

namespace SchoolAccount.Application.Pipelines.Query;

public class DynamicQueryPipeline<TRow>(IList<IQueryFactory<TRow>> factories)
    : IQueryFactoryPipeline<TRow>
    where TRow : IQueryRow
{
    public DynamicQueryPipeline(params IQueryFactory<TRow>[] factories) : this(factories.ToList())
    { }
    
    public IList<IQueryFactory<TRow>> Factories { get; } = factories;

    public static implicit operator DynamicQueryPipeline<TRow>(Collection<IQueryFactory<TRow>> factories)
    {
        return new DynamicQueryPipeline<TRow>(factories);
    }
    
    [SuppressMessage("Design", "CA1002:Do not expose generic lists")]
    public static implicit operator DynamicQueryPipeline<TRow>(List<IQueryFactory<TRow>> factories)
    {
        return new DynamicQueryPipeline<TRow>(factories);
    }
}