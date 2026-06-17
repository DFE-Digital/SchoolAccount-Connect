using SchoolAccount.Application.Features.Tasks.Search;

namespace SchoolAccount.Tests.Common.Builders.Tasks.Search;

public class SearchTasksResponseTaskBuilder
{
    private long _id = 1;
    private string _name = "Test Task";
    private string? _description;

    public static SearchTasksResponseTaskBuilder AResponseTask() => new();

    public SearchTasksResponseTaskBuilder WithId(long id)
    {
        _id = id;
        return this;
    }

    public SearchTasksResponseTaskBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public SearchTasksResponseTaskBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public SearchTasksResponseTask Build() =>
        new()
        {
            Id = _id,
            Name = _name,
            Description = _description,
        };

    public static implicit operator SearchTasksResponseTask(SearchTasksResponseTaskBuilder builder) => builder.Build();
}
