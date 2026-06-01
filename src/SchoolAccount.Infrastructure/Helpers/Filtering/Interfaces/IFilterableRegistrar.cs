using SchoolAccount.Application.Abstractions;
using SchoolAccount.Kernel;

namespace SchoolAccount.Infrastructure.Helpers.Filtering.Interfaces;

public interface IFilterableRegistrar
{
    Type TypeBeingRegistered { get; }
    FieldSelector FieldSelectorsBeingRegistered { get; }
}

public interface IFilterableRegistrar<in TFilter> : IFilterableRegistrar
    where TFilter : IFilter
{
    void ConsolidateFilters(TFilter filter);
}
