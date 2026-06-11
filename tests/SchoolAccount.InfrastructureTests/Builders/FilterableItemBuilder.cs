using SchoolAccount.Application.Extensions;
using SchoolAccount.Application.Features.Shared.Filtering;

namespace SchoolAccount.InfrastructureTests.Builders;

public sealed class FilterableItemBuilder
{
    private readonly string _displayValue;
    private readonly string _value;

    private bool _isSelected;
    private int? _count;

    internal FilterableItemBuilder(string displayValue, string value)
    {
        _displayValue = displayValue;
        _value = value;
    }

    public static FilterableItemBuilder AFilterItem(string displayValue, string value)
    {
        return new FilterableItemBuilder(displayValue, value);
    }

    public static FilterableItemBuilder AFilterItem(string value)
    {
        return new FilterableItemBuilder(value, value);
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

    public FilterableItem Build()
    {
        return new FilterableItem
        {
            DisplayName = _displayValue,
            Value = _value,
            IsSelected = _isSelected,
            Count = _count,
        };
    }

    public static implicit operator FilterableItem(FilterableItemBuilder builder)
    {
        return builder.Build();
    }
}
