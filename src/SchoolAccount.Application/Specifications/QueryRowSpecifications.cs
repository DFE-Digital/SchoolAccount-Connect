using System.Linq.Expressions;
using SchoolAccount.Application.Features.Shared.Query.Interfaces;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Specifications;

public static class QueryRowSpecifications
{
    public static Expression<Func<TRow, bool>> IsWithinDateRange<TRow>(DateOnly rangeStart, DateOnly rangeEnd)
        where TRow : IQueryRow
    {
        return x => x.SortDate.HasValue && x.SortDate.Value >= rangeStart && x.SortDate.Value <= rangeEnd;
    }

    public static Expression<Func<TRow, bool>> IsWithinDateRange<TRow>(DateOnlyRange range)
        where TRow: IQueryRow
    {
        return IsWithinDateRange<TRow>(range.Start, range.End);
    }
}
