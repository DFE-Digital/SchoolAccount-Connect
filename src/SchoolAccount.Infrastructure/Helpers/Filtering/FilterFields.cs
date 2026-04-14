using SchoolAccount.Application.Features.Shared.Filtering;
using SchoolAccount.Infrastructure.Helpers.Filtering.Interfaces;

namespace SchoolAccount.Infrastructure.Helpers.Filtering;

public static class FilterFields
{
    public static async Task<List<Filterable>> GetAvailableFiltersAsync<TRow>(
        FilterableEntities identifier,
        IEnumerable<IFilterableFactory> factories,
        IQueryable<TRow>? baseQuery = null) where TRow : class
    {
        var applicableFactories = factories
            .OfType<IFilterableFactory<TRow>>()
            .Where(f => f.IsCreatorFor(identifier))
            .ToList();

        if (applicableFactories.Count == 0)
        {
            return [];
        }

        var results = await Task.WhenAll(
            applicableFactories.Select(f => f.GetAvailableFiltersAsync(baseQuery))
        );

        return results.SelectMany(x => x).ToList();
    }
}