using System.Diagnostics.CodeAnalysis;

namespace SchoolAccount.Domain.Tasks;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
public class TaskRelationEntity
{
    public long Id { get; set; }

    public long TaskId { get; set; }

    public long RelatedTaskId { get; set; }

    public int RelatedOrder { get; set; }

    public virtual TaskEntity Task { get; set; } = null!;

    public virtual TaskEntity RelatedTask { get; set; } = null!;
}
