using SchoolAccount.Domain.Conditions;

namespace SchoolAccount.Domain.Subtasks;

public class SubTaskConditionEntity
{
    public long Id { get; set; }
    public long SubTaskId { get; set; }
    public int ConditionId { get; set; }
    
    public ConditionComparitorType Comparitor { get; set; }
    public string? Value { get; set; }

    public virtual SubTaskEntity SubTask { get; set; } = null!;
    public virtual ConditionEntity Condition { get; set; } = null!;
}