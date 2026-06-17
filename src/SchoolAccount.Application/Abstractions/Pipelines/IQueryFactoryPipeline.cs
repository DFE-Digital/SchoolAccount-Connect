using SchoolAccount.Application.Features.Shared.Query.Interfaces;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Abstractions.Pipelines;

public interface IQueryFactoryPipeline<TRow> 
    where TRow : IQueryRow
{
    IList<IQueryFactory<TRow>> Factories { get; }
}