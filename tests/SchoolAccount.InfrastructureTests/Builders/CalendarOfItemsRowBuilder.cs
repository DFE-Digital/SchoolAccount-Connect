using SchoolAccount.Application.Features.CalendarOfItems.Common.Enums;
using SchoolAccount.Application.Features.CalendarOfItems.Common.Models;

namespace SchoolAccount.InfrastructureTests.Builders;

public sealed class CalendarOfItemsRowBuilder
{
    private readonly long _id;
    private readonly string _title;
    private readonly DateOnly? _sortDate;
    private readonly CalendarOfItemsRowType _type;
    private readonly List<CalendarOfItemsExtensionNode> _types = [];
    private readonly List<CalendarOfItemsExtensionNode> _tags = [];

    private string? _description;
    private CalendarOfItemsRowStatus? _status;

    internal CalendarOfItemsRowBuilder(long id, string title, DateOnly? sortDate, CalendarOfItemsRowType type)
    {
        _id = id;
        _title = title;
        _sortDate = sortDate;
        _type = type;
    }

    public CalendarOfItemsRowBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public CalendarOfItemsRowBuilder WithStatus(CalendarOfItemsRowStatus status)
    {
        _status = status;
        return this;
    }

    public CalendarOfItemsRowBuilder WithTypes(IEnumerable<CalendarOfItemsExtensionNode> types)
    {
        _types.AddRange(types);
        return this;
    }

    public CalendarOfItemsRowBuilder WithTypes(params CalendarOfItemsExtensionNode[] types)
    {
        _types.AddRange(types);
        return this;
    }

    public CalendarOfItemsRowBuilder WithTags(IEnumerable<CalendarOfItemsExtensionNode> tags)
    {
        _tags.AddRange(tags);
        return this;
    }

    public CalendarOfItemsRowBuilder WithTags(params CalendarOfItemsExtensionNode[] tags)
    {
        _tags.AddRange(tags);
        return this;
    }

    public CalendarOfItemsRow Build()
    {
        return new CalendarOfItemsRow
        {
            Id = _id,
            Name = _title,
            SortDate = _sortDate,
            Type = _type,
            Description = _description,
            Types = _types,
            Tags = _tags,
        };
    }

    public static implicit operator CalendarOfItemsRow(CalendarOfItemsRowBuilder builder)
    {
        return builder.Build();
    }
}
