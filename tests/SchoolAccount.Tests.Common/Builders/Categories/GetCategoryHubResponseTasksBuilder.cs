using SchoolAccount.Application.Features.Categories.GetCategoryHub;
using SchoolAccount.Domain.Common;

namespace SchoolAccount.Tests.Common.Builders.Categories;

public class GetCategoryHubResponseTaskBuilder
{
    private long _id = 1;
    private string _name = "Test Task";
    private Requirement _requirement = Requirement.None;

    public static GetCategoryHubResponseTaskBuilder AResponseTask() => new();

    public GetCategoryHubResponseTaskBuilder WithId(long id)
    {
        _id = id;
        return this;
    }

    public GetCategoryHubResponseTaskBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public GetCategoryHubResponseTaskBuilder WithRequirement(Requirement requirement)
    {
        _requirement = requirement;
        return this;
    }

    public GetCategoryHubResponseTasks Build() =>
        new()
        {
            Id = _id,
            Name = _name,
            Requirement = _requirement,
        };

    public static implicit operator GetCategoryHubResponseTasks(GetCategoryHubResponseTaskBuilder builder) =>
        builder.Build();
}
