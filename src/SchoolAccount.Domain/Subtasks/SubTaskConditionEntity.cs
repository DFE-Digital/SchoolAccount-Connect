namespace SchoolAccount.Domain.Subtasks;

public class SubTaskConditionEntity
{
    public long Id { get; set; }
    public long SubTaskId { get; set; }
    public string Identifier { get; set; } = null!;
    public SubTaskConditionComparitorType Comparitor { get; set; }
    public string? Value { get; set; }

    public virtual SubTaskEntity SubTask { get; set; } = null!;
}

public enum SubTaskConditionComparitorType
{
    NotDefined = 0,
    Equal = 1,
    NotEqual,
    LessThan,
    LessThanOrEqual,
    GreaterThan,
    GreaterThanOrEqual,
} 