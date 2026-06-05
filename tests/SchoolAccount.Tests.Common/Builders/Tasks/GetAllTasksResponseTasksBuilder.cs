using SchoolAccount.Application.Features.Tasks.GetAll;
using SchoolAccount.Domain.Common;

namespace SchoolAccount.Tests.Common.Builders.Tasks;

public class GetAllTasksResponseTasksBuilder
{
    private long _id = 1;
    private string _name = "Test Task";
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

    public GetAllTasksResponseTasksBuilder WithRequirement(Requirement requirement)
    {
        _requirement = requirement;
        return this;
    }

    public GetAllTasksResponseTasks Build() =>
        new()
        {
            Id = _id,
            Name = _name,
            Requirement = _requirement,
        };

    public static implicit operator GetAllTasksResponseTasks(GetAllTasksResponseTasksBuilder builder) =>
        builder.Build();
}
