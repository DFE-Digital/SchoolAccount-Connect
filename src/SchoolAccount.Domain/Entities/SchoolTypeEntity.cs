using System.Diagnostics.CodeAnalysis;

namespace SchoolAccount.Domain.Entities;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
public class SchoolTypeEntity
{
    public int Id { get; init; }

    public required string Name { get; set; }

    public virtual ICollection<SchoolTypeTagMappingEntity> SchoolTypeTagMappings { get; } = [];
}
