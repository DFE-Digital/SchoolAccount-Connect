using System.Collections.ObjectModel;

namespace SchoolAccount.Application.Features.Shared.Filtering;

public class FilterableItem
{
    public string DisplayName { get; init; } = null!;
    public string Value { get; init; } = null!;
    public bool IsSelected { get; set; }
    public Collection<FilterableItem>? Children { get; init; }
    public int? Count { get; init; }
}