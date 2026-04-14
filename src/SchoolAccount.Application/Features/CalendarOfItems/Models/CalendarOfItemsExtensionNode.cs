namespace SchoolAccount.Application.Features.CalendarOfItems.Models;

public class CalendarOfItemsExtensionNode
{
    public long Id { get; init; }
    public string? Name { get; init; }
    public string? DisplayValue { get; init; }
    public CalendarOfItemsExtensionNodeType Type { get; init; } = CalendarOfItemsExtensionNodeType.NotSpecified;
}

public enum CalendarOfItemsExtensionNodeType
{
    NotSpecified = 0,
    Tag = 1,
    Type = 2,
}