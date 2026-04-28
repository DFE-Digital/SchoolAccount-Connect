using SchoolAccount.Application.Features.CalendarOfItems.Enums;
using SchoolAccount.Application.Features.CalendarOfItems.Models;

namespace SchoolAccount.InfrastructureTests.Builders;

public sealed class CalendarOfItemsRowStatusBuilder
{
    private readonly string _displayValue;
    private CalendarOfItemsRowType? _type;
    private string? _theme;
    private long? _entityId;

    internal CalendarOfItemsRowStatusBuilder(string displayValue)
    {
        _displayValue = displayValue;
    }

    public CalendarOfItemsRowStatusBuilder WithType(CalendarOfItemsRowType type)
    {
        _type = type;
        return this;
    }

    public CalendarOfItemsRowStatusBuilder WithTheme(string theme)
    {
        _theme = theme;
        return this;
    }

    public CalendarOfItemsRowStatusBuilder WithEntity(long entityId)
    {
        _entityId = entityId;
        return this;
    }

    public CalendarOfItemsRowStatus Build()
    {
        return new CalendarOfItemsRowStatus
        {
            DisplayValue = _displayValue,
            Type = _type,
            Theme = _theme,
            EntityId = _entityId,
        };
    }

    public static implicit operator CalendarOfItemsRowStatus(CalendarOfItemsRowStatusBuilder builder)
    {
        return builder.Build();
    }
}
