using System.Collections.ObjectModel;

namespace SchoolAccount.Application.Features.Shared.Filtering;

public record FilterableItem
{
    public string DisplayName { get; init; } = null!;
    public string Value { get; init; } = null!;
    public bool IsSelected { get; set; }
    public int? Count { get; init; }
}
