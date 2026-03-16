using SchoolAccount.Infrastructure.Models.Interfaces;

namespace SchoolAccount.Infrastructure.Models.Entities;

public class WorkflowStateEntity : IIdNameEntity<int>
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<SubTaskEntity> SubTasks { get; } = [];

    public virtual ICollection<TaskEntity> Tasks { get; } = [];
}
