using SchoolAccount.Application.Extensions;
using SchoolAccount.Application.Features.Shared.Filtering.Models;

namespace SchoolAccount.InfrastructureTests.Builders;

public sealed class FilterableBuilder
{
    private readonly string _id;
    private readonly List<FilterableItemBuilder> _items = [];
    private string? _displayName;

    internal FilterableBuilder(string id)
    {
        _id = id;
    }

    public FilterableBuilder WithDisplayName(string displayName)
    {
        _displayName = displayName;
        return this;
    }

    public FilterableBuilder WithValues(IEnumerable<FilterableItemBuilder> items)
    {
        _items.AddRange(items);
        return this;
    }

    public FilterableBuilder WithValues(params FilterableItemBuilder[] items)
    {
        _items.AddRange(items);
        return this;
    }

    public Filterable Build()
    {
        return new Filterable(_id, _displayName ?? string.Empty)
        {
            Values = _items.Select(x => x.Build()).ToCollection(),
        };
    }

    public static implicit operator Filterable(FilterableBuilder builder)
    {
        return builder.Build();
    }
}
