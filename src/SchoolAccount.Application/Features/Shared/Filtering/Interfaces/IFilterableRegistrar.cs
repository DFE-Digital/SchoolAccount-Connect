using SchoolAccount.Application.Features.Shared.Filtering.Models;

namespace SchoolAccount.Application.Features.Shared.Filtering.Interfaces;

public interface IFilterableRegistrar
{
    Type TypeBeingRegistered { get; }
    FieldSelector FieldSelectorsBeingRegistered { get; }
}

public interface IFilterableAndConsolidateRegistrar : IFilterableRegistrar
{
    void ConsolidateFilters(IList<FilterRequest> filter);
}
