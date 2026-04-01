using SchoolAccount.Application.Features.CalendarOfItems.Models;
using SchoolAccount.Application.Features.Shared.Filtering;
using SchoolAccount.Infrastructure.Helpers.Filtering.Interfaces;
using SchoolAccount.Infrastructure.Helpers.Filtering.Models;

namespace SchoolAccount.Infrastructure.Helpers.Filtering;

public static class FilterFields
{
    public static async Task<List<Filterable>> GetAvailableFiltersAsync(
        FilterableEntities identifier,
        IEnumerable<IFilterableFactory> factories,
        IQueryable<CalendarOfItemsRow>? baseQuery = null
    )
    {
        var applicableFactories = factories.Where(f => f.IsCreatorFor(identifier)).ToList();

        if (applicableFactories.Count == 0)
        {
            return [];
        }

        var results = await Task.WhenAll(applicableFactories.Select(f => f.GetAvailableFiltersAsync(baseQuery)));

        return results.SelectMany(x => x).ToList();
    }
}
