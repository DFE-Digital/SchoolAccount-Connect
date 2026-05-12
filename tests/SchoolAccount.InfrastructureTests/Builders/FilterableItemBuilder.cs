using SchoolAccount.Application.Extensions;
using SchoolAccount.Application.Features.Shared.Filtering;

namespace SchoolAccount.InfrastructureTests.Builders;

public sealed class FilterableItemBuilder
{
    private readonly string _displayValue;
    private readonly string _value;

    private bool _isSelected;
    private List<FilterableItem>? _children;
    private int? _count;

    internal FilterableItemBuilder(string displayValue, string value)
    {
        _displayValue = displayValue;
        _value = value;
    }

    public FilterableItemBuilder IsSelected()
    {
        _isSelected = true;
        return this;
    }

    public FilterableItemBuilder UnSelected()
    {
        _isSelected = false;
        return this;
    }

    public FilterableItemBuilder WithCount(int count)
    {
        _count = count;
        return this;
    }

    public FilterableItemBuilder WithChild(IEnumerable<FilterableItem> children)
    {
        _children ??= [];
        _children.AddRange(children);
        return this;
    }

    public FilterableItemBuilder WithChild(params FilterableItem[] children)
    {
        _children ??= [];
        _children.AddRange(children);
        return this;
    }

    public FilterableItemBuilder WithChild(IEnumerable<FilterableItemBuilder> children)
    {
        _children ??= [];
        _children.AddRange(children.Select(x => x.Build()));
        return this;
    }

    public FilterableItemBuilder WithChild(params FilterableItemBuilder[] children)
    {
        _children ??= [];
        _children.AddRange(children.Select(x => x.Build()));
        return this;
    }

    public FilterableItem Build()
    {
        return new FilterableItem
        {
            DisplayName = _displayValue,
            Value = _value,
            IsSelected = _isSelected,
            Children = _children?.ToCollection() ?? null,
            Count = _count,
        };
    }

    public static implicit operator FilterableItem(FilterableItemBuilder builder)
    {
        return builder.Build();
    }
}
