using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Application.Abstractions.Pipelines;
using SchoolAccount.Application.Features.Shared.Filtering.Filters;
using SchoolAccount.Application.Features.Shared.Filtering.Interfaces;

namespace SchoolAccount.Application.Pipelines.Filters;

public class CalendarOfItemsFilterPipeline(
    IApplicationDbContext applicationDbContext
) : IFilterablePipeline
{
    public IList<IFilterableFactory> Factories { get; } =
    [
        new SubTaskFilterableFactory(applicationDbContext)
    ];
}