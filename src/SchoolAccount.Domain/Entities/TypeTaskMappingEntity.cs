using System.Diagnostics.CodeAnalysis;

namespace SchoolAccount.Domain.Entities;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
public class TypeTaskMappingEntity
{
    public int Id { get; set; }

    public long TaskId { get; set; }

    public int TypeId { get; set; }

    public virtual required TaskEntity Task { get; set; }

    public virtual required TypeEntity Type { get; set; }
}
