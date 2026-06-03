using SchoolAccount.Application.Features.Shared.Filtering.Models;
using SchoolAccount.Application.Features.Shared.Query.Interfaces;

namespace SchoolAccount.Application.Features.Shared.Filtering.Interfaces;

public interface IFilterableFactory<in TRow>
    where TRow: IQueryRow
{
    Task<List<Filterable>> GetAvailableFiltersAsync(IQueryable<TRow>? baseQuery = null);
}
