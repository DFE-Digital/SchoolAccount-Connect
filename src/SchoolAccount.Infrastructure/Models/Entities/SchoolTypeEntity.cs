using SchoolAccount.Infrastructure.Models.Interfaces;
using SchoolAccount.Kernel;

namespace SchoolAccount.Infrastructure.Models.Entities;

public class SchoolTypeEntity : IIdNameEntity<SchoolType>
{
    public SchoolType Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<SchoolTypeTagMappingEntity> SchoolTypeTagMappings { get; } = [];
}