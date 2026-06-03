using SchoolAccount.Application.Features.Shared.Filtering;
using SchoolAccount.Application.Features.Shared.Filtering.Models;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Features.Shared.Query.Interfaces;

public interface IQueryFactory<TEntity, out TRow>
    where TEntity : IEntity
    where TRow : IQueryRow
{
    Type TypeBeingRegistered => typeof(TEntity);
    IQueryable<TRow> Query(IList<FilterRequest> filter, FieldSelectorMapping mappings);
}
