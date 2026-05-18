using SchoolAccount.Domain.Conditions;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Features.SubTask;

public sealed record GetSubTasksForCardsResponse
{
    public IReadOnlyCollection<GetSubTasksForCardsResponseSubTask> SubTasks { get; init; } = [];
}

public sealed record GetSubTasksForCardsResponseSubTask
{
    public long Id { get; init; }
    public long ParentId { get; init; }
    public string Name { get; init; } = null!;
    public string Description { get; set; } = null!;
    
    public DateTime? CreatedOn { get; init; }
    public DateTime? LastUpdated { get; init; }
    public DateOnlyNullableRange Runtime { get; init; }
    
    public IReadOnlyCollection<GetSubTasksForCardsResponseNode> Types { get; init; } = [];
    public IReadOnlyCollection<GetSubTasksForCardsResponseNode> Tags { get; init; } = [];
    public GetSubTasksForCardsResponseNode? Status { get; init; }
    
    public IReadOnlyCollection<GetSubTasksForCardsResponseCondition> Condition { get; init; } = [];
}

public sealed record GetSubTasksForCardsResponseNode
{
    public string DisplayValue { get; init; } = null!;
    public object? Identifier { get; init; }
    public string? Name { get; init; }
    public long? EntityId { get; init; }
    public string? Colour { get; init; }
    public string? Group { get; init; }
}

public sealed record GetSubTasksForCardsResponseCondition : IConditionObject
{
    public object? Value { get; init; }
    public ConditionComparitorType ComparitorType { get; init; }
    public string Identifier { get; set; } = null!;
}