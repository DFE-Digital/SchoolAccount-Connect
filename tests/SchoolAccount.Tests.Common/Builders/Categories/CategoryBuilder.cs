using SchoolAccount.Domain.Tasks;
using SchoolAccount.Domain.Types;
using SchoolAccount.Tests.Common.Builders.Tasks;

namespace SchoolAccount.Tests.Common.Builders.Categories;

public sealed class CategoryBuilder
{
    private int _id = 1;
    private string _name = "test-category";
    private string _displayName = "Test category";
    private string _tagName = "TEST-TAG";
    private string? _description;
    private string? _hubViewDescription;
    private readonly List<TaskEntity> _tasks = [];
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

    public CategoryBuilder WithTasks(params TaskBuilder[] builders)
    {
        foreach (var builder in builders)
        {
            _tasks.Add(builder.Build());
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
        var type = new TypeEntity
        {
            Id = _id,
            Name = _name,
            TagName = _tagName,
            DisplayName = _displayName,
            Description = _description,
            HubViewDescription = _hubViewDescription,
        };

        foreach (var task in _tasks)
        {
            type.Tasks.Add(task);
        }

        foreach (var child in _children)
        {
            type.Children.Add(child);
        }

        return type;
    }
}
