using System.Collections.ObjectModel;

namespace SchoolAccount.Application.Features.Shared.Filtering;

public class FilterRequest
{
    public JoinType Join { get; init; } = JoinType.And;
    public Collection<FilterRequest> Children { get; init; } = [];

    public string Field { get; init; } = null!;
    public ComparisonType Operator { get; init; } = ComparisonType.Equals;
    public object? Value { get; init; }
}