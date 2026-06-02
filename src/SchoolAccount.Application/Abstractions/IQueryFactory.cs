using SchoolAccount.Application.Features.CalendarOfItems.Models;
using SchoolAccount.Application.Features.Shared.Filtering;
using SchoolAccount.Application.Features.Shared.Filtering.Models;

namespace SchoolAccount.Application.Abstractions;

public interface IQueryFactory<out TRow>
    where TRow: IQueryRow
{
    IQueryable<TRow> Query(IList<FilterRequest> filter, FieldSelectorMapping mappings);
}
