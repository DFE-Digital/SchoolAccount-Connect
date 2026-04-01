using SchoolAccount.Domain.Interfaces;

namespace SchoolAccount.Domain.Entities;

public class TypeTaskMappingEntity : IDatabaseEntity
{
    public int Id { get; set; }

    public long TaskId { get; set; }

    public int TypeId { get; set; }

    public virtual TaskEntity Task { get; set; } = null!;

    public virtual TypeEntity Type { get; set; } = null!;
}
