using SchoolAccount.Application.Features.Shared.Filtering;
using SchoolAccount.Application.Features.Shared.Filtering.Models;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Features.Shared.Query.Interfaces;

public interface IQueryFactory<out TRow> 
    where TRow : IQueryRow
{
    Type? TypeBeingRegistered { get; }
    IQueryable<TRow> Query(IList<FilterRequest> filter, FieldSelectorMapping mappings);
}
