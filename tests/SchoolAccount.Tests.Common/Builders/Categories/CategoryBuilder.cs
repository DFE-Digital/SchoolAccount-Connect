using SchoolAccount.Domain.Types;

namespace SchoolAccount.Tests.Common.Builders.Categories;

public sealed class CategoryBuilder
{
    private int _id = 1;
    private string _name = "test-category";
    private string _displayName = "Test category";
    private string _tagName = "TEST-TAG";
    private string? _description;
    private string? _hubViewDescription;
    private readonly List<TypeTaskMappingEntity> _typeTaskMappings = [];
    private readonly List<TypeEntity> _children = [];

    public static CategoryBuilder ACategory() => new();

    public CategoryBuilder WithId(int id)
    {
        _id = id;
        return this;
    }

    public CategoryBuilder Named(string name)
    {
        _name = name;
        return this;
    }

    public CategoryBuilder WithTagName(string tagName)
    {
        _tagName = tagName;
        return this;
    }

    public CategoryBuilder WithDescription(string? description)
    {
        _description = description;
        return this;
    }

    public CategoryBuilder WithHubViewDescription(string hubViewDescription)
    {
        _hubViewDescription = hubViewDescription;
        return this;
    }

    public CategoryBuilder WithTypeTaskMapping(TypeTaskMappingEntity entity)
    {
        _typeTaskMappings.Add(entity);
        return this;
    }

    public CategoryBuilder WithSubTasks(params TypeTaskMappingBuilder[] builders)
    {
        foreach (var b in builders)
        {
            _typeTaskMappings.Add(b.Build());
        }

        return this;
    }

    public CategoryBuilder WithChildren(CategoryBuilder builder)
    {
        _children.Add(builder.Build());
        return this;
    }

    public TypeEntity Build()
    {
        var task = new TypeEntity
        {
            Id = _id,
            Name = _name,
            TagName = _tagName,
            DisplayName = _displayName,
            Description = _description,
            HubViewDescription = _hubViewDescription,
        };

        foreach (var typeTaskMapping in _typeTaskMappings)
        {
            task.TypeTaskMappings.Add(typeTaskMapping);
        }

        foreach (var child in _children)
        {
            task.Children.Add(child);
        }

        return task;
    }
}
