using SchoolAccount.Domain.Subtasks;

namespace SchoolAccount.Domain.Conditions;

public class ConditionEntity
{
    public int Id { get; set; }
    public string Identifier { get; set; } = null!;

    public virtual ICollection<SubTaskConditionEntity> SubTaskConditions { get; } = [];
}