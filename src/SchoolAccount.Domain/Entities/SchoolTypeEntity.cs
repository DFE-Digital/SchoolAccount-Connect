using SchoolAccount.Domain.Interfaces;

namespace SchoolAccount.Domain.Entities;

public class SchoolTypeEntity : IIdNameEntity<int>
{
    public int Id { get; init; }

    public string Name { get; set; } = null!;

    public virtual ICollection<SchoolTypeTagMappingEntity> SchoolTypeTagMappings { get; } = [];
}
