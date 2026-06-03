using System.Collections.ObjectModel;
using SchoolAccount.Application.Features.Shared.Filtering.Interfaces;
using SchoolAccount.Application.Features.Shared.Filtering.Models;
using SchoolAccount.Application.Features.Shared.Query.Interfaces;

namespace SchoolAccount.Application.Features.Shared.Filtering;

public static class FilterFields
{
    public static async Task<List<Filterable>> GetAvailableFiltersAsync<TRow>(
        Collection<IFilterableFactory> factories,
        IQueryable<TRow>? baseQuery = null
    )
        where TRow : IQueryRow
    {
        if (factories.Count == 0)
        {
            return [];
        }

        var results = await Task.WhenAll(factories.Select(f => f.GetAvailableFiltersAsync(baseQuery)));
        return results.SelectMany(x => x).ToList();
    }
}
