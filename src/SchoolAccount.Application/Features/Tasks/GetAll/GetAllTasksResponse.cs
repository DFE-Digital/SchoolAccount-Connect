using SchoolAccount.Application.Common;
using SchoolAccount.Domain.Common;

namespace SchoolAccount.Application.Features.Tasks.GetAll;

public sealed record GetAllTasksResponse
{
    public PagedResult<GetAllTasksResponseTask> Tasks { get; init; } = new();
};

public sealed record GetAllTasksResponseTask
{
    public long Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }

    public Requirement? Requirement { get; init; }
}
