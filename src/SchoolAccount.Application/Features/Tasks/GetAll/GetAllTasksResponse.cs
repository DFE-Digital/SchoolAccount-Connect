using SchoolAccount.Domain.Common;
using X.PagedList;

namespace SchoolAccount.Application.Features.Tasks.GetAll;

public sealed record GetAllTasksResponse(IReadOnlyCollection<GetAllTasksResponseTasks> Tasks);

public sealed record GetAllTasksResponseTasks
{
    public long Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }

    public Requirement? Requirement { get; init; }
}
