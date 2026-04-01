using SchoolAccount.Domain.Entities;

namespace SchoolAccount.Domain.Models.Entities;

public partial class WorkflowStateEntity
{
    public int Id { get; init; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<SubTaskEntity> SubTasks { get; } = [];

    public virtual ICollection<TaskEntity> Tasks { get; } = [];
}
