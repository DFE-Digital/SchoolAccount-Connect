using SchoolAccount.Infrastructure.Models.Interfaces;

namespace SchoolAccount.Infrastructure.Models.Entities;

public class SchoolTypeTagMappingEntity : IDatabaseEntity
{
    public int Id { get; set; }

    public int SchoolTypeId { get; set; }

    public long TagId { get; set; }

    public virtual SchoolTypeEntity SchoolType { get; } = null!;

    public virtual TagEntity Tag { get; } = null!;
}
