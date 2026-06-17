using System.Collections;
using SchoolAccount.Application.Features.Shared.Filtering.Interfaces;

namespace SchoolAccount.Application.Abstractions.Pipelines;

public interface IFilterablePipeline
{
    IList<IFilterableFactory> Factories { get; }
}