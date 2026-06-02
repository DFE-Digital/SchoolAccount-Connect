using SchoolAccount.Application.Features.CalendarOfItems.Models;
using SchoolAccount.Application.Features.Shared.Filtering.Models;

namespace SchoolAccount.Application.Features.Shared.Filtering.Interfaces;

public interface IFilterableFactory<in TRow>
    where TRow: IQueryRow
{
    Task<List<Filterable>> GetAvailableFiltersAsync(IQueryable<TRow>? baseQuery = null);
}
