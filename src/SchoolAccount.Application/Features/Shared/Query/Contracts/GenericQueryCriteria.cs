using SchoolAccount.Application.Features.Shared.Filtering.Models;
using SchoolAccount.Application.Features.Shared.Query.Delegates;
using SchoolAccount.Application.Features.Shared.Query.Interfaces;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Features.Shared.Query.Contracts;

public class GenericQueryCriteria<TRow> : IQueryCriteria<TRow>
    where TRow : IQueryRow
{
    public int PageSize { get; init; }
    public int PageNumber { get; init; }
    public DateOnlyRange Range { get; init; }
    public IList<FilterRequest> Filter { get; init; } = [];
    public bool PopulateFilterOptions { get; init; } = true;
    public GenericOrderFunction<TRow>? CustomOrderByFunction { get; init; }
}
