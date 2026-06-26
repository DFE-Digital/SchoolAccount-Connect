using SchoolAccount.Application.Features.Shared.Filtering;
using SchoolAccount.Application.Features.Shared.Filtering.Models;
using SchoolAccount.Application.Features.Shared.Query.Contracts;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Features.Shared.Query.Interfaces;

public interface IQueryFactory<TRow> 
    where TRow : IQueryRow
{
    Type? TypeBeingRegistered { get; }

    Task<QueryResponse<TRow>> Query(GenericQueryCriteria<TRow> criteria, FieldSelectorMapping mappings,
        CancellationToken cancellationToken);
}

public record QueryResponse<TRow>(int Count, IEnumerable<TRow> Payload)
    where TRow : IQueryRow
{
    public static implicit operator QueryResponse<TRow>(Tuple<int, IEnumerable<TRow>> tuple) =>
        new(tuple.Item1, tuple.Item2);
    public static implicit operator QueryResponse<TRow>((int, IEnumerable<TRow>) tuple) =>
        new(tuple.Item1, tuple.Item2);
}