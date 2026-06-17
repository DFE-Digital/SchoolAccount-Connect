using SchoolAccount.Application.Abstractions.Pipelines;
using SchoolAccount.Application.Features.Shared.Filtering.Interfaces;

namespace SchoolAccount.Application.Pipelines.Filters;

public class EmptyFilterPipeline : IFilterablePipeline
{
    public IList<IFilterableFactory> Factories { get; } = [];
}