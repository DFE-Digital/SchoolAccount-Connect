using SchoolAccount.Domain.Common;

namespace SchoolAccount.Application.Features.Tasks.GetById;

public sealed record GetTaskByIdResponse
{
    public long Id { get; init; }

    public string? ReferenceNo { get; init; }

    public string Name { get; init; } = string.Empty;

    public Requirement? Requirement { get; init; }

    public DateTime DateUpdated { get; init; }

    public string UpdatedBy { get; init; } = string.Empty;

    public DateTime? SubTaskLastUpdated => GetSubTaskLastUpdated();

    public IReadOnlyCollection<GetTaskByIdResponseSubtask> SubTasks { get; init; } = [];

    public IReadOnlyCollection<GetTaskByIdResponseResource> Resources { get; init; } = [];

    public IReadOnlyCollection<GetTaskByIdResponseRelatedTask> RelatedTasks { get; init; } = [];

    private DateTime? GetSubTaskLastUpdated()
    {
        return SubTasks.OrderByDescending(st => st.DateUpdated).FirstOrDefault()?.DateUpdated;
    }
}

public sealed record GetTaskByIdResponseSubtask
{
    public long Id { get; init; }

    public string? ReferenceNo { get; init; }

    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }

    public DateOnly? StartDate { get; init; }

    public bool? StartDateIsExact { get; init; }

    public DateOnly? DueDate { get; init; }

    public bool? DueDateIsExact { get; init; }

    public string AvailabilityLabel { get; init; } = string.Empty;

    public string DueDateLabel { get; init; } = string.Empty;

    public Requirement Requirement { get; init; }

    public WorkflowState WorkflowState { get; init; }

    public DateTime DateUpdated { get; init; }

    public string UpdatedBy { get; init; } = string.Empty;

    public bool HasDueDateLabel => !string.IsNullOrWhiteSpace(DueDateLabel);

    public bool HasAvailabilityLabel => !string.IsNullOrWhiteSpace(AvailabilityLabel);

    public bool HasDescription => !string.IsNullOrWhiteSpace(Description);

    public bool IsOptional => Requirement == Requirement.Optional;

    public DateOnly? SortingDate => DueDate ?? StartDate;

    public string? ResourceName { get; init; } = string.Empty;

    public string? ResourceLink { get; init; } = string.Empty;

    public bool HasResourceLink => !string.IsNullOrWhiteSpace(ResourceLink);
}

public sealed record GetTaskByIdResponseResource
{
    public required string Name { get; init; }

    public string? Link { get; init; }

    public bool HasLink => !string.IsNullOrWhiteSpace(Link);
}

public sealed record GetTaskByIdResponseRelatedTask
{
    public long Id { get; init; }

    public string Name { get; init; } = string.Empty;
}
