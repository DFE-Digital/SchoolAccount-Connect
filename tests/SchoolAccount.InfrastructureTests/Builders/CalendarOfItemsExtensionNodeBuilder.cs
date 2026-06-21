using SchoolAccount.Application.Features.Calendars.CalendarList.Models;

namespace SchoolAccount.InfrastructureTests.Builders;

public sealed class CalendarOfItemsExtensionNodeBuilder
{
    private readonly long _id;
    private string? _name;
    private string? _displayValue;
    private CalendarOfItemsExtensionNodeType _type = CalendarOfItemsExtensionNodeType.NotSpecified;

    internal CalendarOfItemsExtensionNodeBuilder(long id)
    {
        _id = id;
    }

    public CalendarOfItemsExtensionNodeBuilder WithType(CalendarOfItemsExtensionNodeType type)
    {
        _type = type;
        return this;
    }

    public CalendarOfItemsExtensionNodeBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public CalendarOfItemsExtensionNodeBuilder WithDisplayValue(string displayValue)
    {
        _displayValue = displayValue;
        return this;
    }

    public CalendarOfItemsExtensionNode Build()
    {
        return new CalendarOfItemsExtensionNode
        {
            Id = _id,
            Name = _name,
            DisplayValue = _displayValue,
            Type = _type,
        };
    }

    public static implicit operator CalendarOfItemsExtensionNode(CalendarOfItemsExtensionNodeBuilder builder)
    {
        return builder.Build();
    }
}
