using System.Diagnostics.CodeAnalysis;

namespace SchoolAccount.Domain.Entities;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
public partial class WorkflowStateEntity
{
    public int Id { get; init; }

    public required string Name { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<SubTaskEntity> SubTasks { get; } = [];

    public virtual ICollection<TaskEntity> Tasks { get; } = [];
}
