using SchoolAccount.Application.Common;

namespace SchoolAccount.Application.Features.Tasks.Search;

public sealed record SearchTasksResponse
{
    public PagedResult<SearchTasksResponseTask> Tasks { get; init; } = new();
};

public sealed record SearchTasksResponseTask
{
    public long Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; } = string.Empty;

    public DateTime DateUpdated { get; init; }
};
