using SchoolAccount.Application.Features.CalendarOfItems.Models;
using SchoolAccount.Application.Features.Shared.Filtering;
using SchoolAccount.Infrastructure.Helpers.Filtering.Models;

namespace SchoolAccount.Infrastructure.Helpers.Filtering.Interfaces;

public interface IFilterableFactory
{
    bool IsCreatorFor(FilterableEntities identifier);

    Task<List<Filterable>> GetAvailableFiltersAsync(IQueryable<CalendarOfItemsRow>? baseQuery = null);
}
