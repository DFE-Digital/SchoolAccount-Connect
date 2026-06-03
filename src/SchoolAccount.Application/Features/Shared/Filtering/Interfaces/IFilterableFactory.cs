using SchoolAccount.Application.Features.Shared.Filtering.Models;
using SchoolAccount.Application.Features.Shared.Query.Interfaces;

namespace SchoolAccount.Application.Features.Shared.Filtering.Interfaces;

public interface IFilterableFactory
{
    Task<List<Filterable>> GetAvailableFiltersAsync<TRow>(IQueryable<TRow>? baseQuery = null)
        where TRow : IQueryRow;
}
