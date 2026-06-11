using System.Collections.ObjectModel;
using SchoolAccount.Application.Extensions;

namespace SchoolAccount.Application.Features.Shared.Filtering;

public record Filterable(FilterableItemType Type, string Field, string DisplayName)
{
    public Collection<FilterableItem> Values { get; init; } = [];

    public bool AnySelectedChildren => Values.Any(HasSelection);

    private static bool HasSelection(FilterableItem item)
    {
        return item.IsSelected;
    }

    public Collection<FilterableItem> GetValuesWithCountOnly(bool includeNullables = false)
    {
        return Values
            .Where(x => x.Count.HasValue && x.Count.Value > 0 || includeNullables && !x.Count.HasValue)
            .ToCollection();
    }

    public Collection<FilterableItem> GetSelectedValuesOnly()
    {
        return Values.Where(x => x.IsSelected).ToCollection();
    }
}

public enum FilterableItemType
{
    Unspecified = 0,
    Checkbox = 1,
}
