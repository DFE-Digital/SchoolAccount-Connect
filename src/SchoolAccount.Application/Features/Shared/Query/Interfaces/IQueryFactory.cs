using SchoolAccount.Application.Features.Shared.Filtering;
using SchoolAccount.Application.Features.Shared.Filtering.Models;

namespace SchoolAccount.Application.Features.Shared.Query.Interfaces;

public interface IQueryFactory<out TRow>
    where TRow: IQueryRow
{
    IQueryable<TRow> Query(IList<FilterRequest> filter, FieldSelectorMapping mappings);
}
