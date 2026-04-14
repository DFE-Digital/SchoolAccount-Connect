using SchoolAccount.Application.Features.Shared.Filtering;

namespace SchoolAccount.Infrastructure.Helpers.Filtering.Interfaces;

public interface IFilterableFactory
{
    bool IsCreatorFor(FilterableEntities identifier);
}

public interface IFilterableFactory<TRow> : IFilterableFactory where TRow : class
{
    Task<List<Filterable>> GetAvailableFiltersAsync(IQueryable<TRow>? baseQuery = null);
}