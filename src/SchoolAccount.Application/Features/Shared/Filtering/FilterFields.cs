using SchoolAccount.Application.Features.CalendarOfItems.Models;
using SchoolAccount.Application.Features.Shared.Filtering.Interfaces;
using SchoolAccount.Application.Features.Shared.Filtering.Models;

namespace SchoolAccount.Application.Features.Shared.Filtering;

public static class FilterFields
{
    public static async Task<List<Filterable>> GetAvailableFiltersAsync<TRow>(
        IList<IFilterableFactory<TRow>> factories,
        IQueryable<TRow>? baseQuery = null
    )
        where TRow: IQueryRow
    {
        if (factories.Count == 0)
        {
            return [];
        }

        var results = await Task.WhenAll(factories.Select(f => f.GetAvailableFiltersAsync(baseQuery)));
        return results.SelectMany(x => x).ToList();
    }
}
