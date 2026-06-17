using SchoolAccount.Application.Features.Tasks.GetAll;
using SchoolAccount.Domain.Common;

namespace SchoolAccount.Tests.Common.Builders.Tasks.GetAll;

public class GetAllTasksResponseTasksBuilder
{
    private long _id = 1;
    private string _name = "Test Task";
    private string? _description;
    private Requirement _requirement = Requirement.None;

    public static GetAllTasksResponseTasksBuilder AResponseTask() => new();

    public GetAllTasksResponseTasksBuilder WithId(long id)
    {
        _id = id;
        return this;
    }

    public GetAllTasksResponseTasksBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public GetAllTasksResponseTasksBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public GetAllTasksResponseTasksBuilder WithRequirement(Requirement requirement)
    {
        _requirement = requirement;
        return this;
    }

    public GetAllTasksResponseTask Build() =>
        new()
        {
            Id = _id,
            Name = _name,
            Description = _description,
            Requirement = _requirement,
        };

    public static implicit operator GetAllTasksResponseTask(GetAllTasksResponseTasksBuilder builder) => builder.Build();
}
