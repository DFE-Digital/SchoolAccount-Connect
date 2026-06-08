using System.Diagnostics.CodeAnalysis;
using SchoolAccount.Domain.Tasks;

namespace SchoolAccount.Domain.Types;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
public class TypeEntity
{
    public int Id { get; set; }

    public required string TagName { get; set; }

    public required string Name { get; set; }

    public required string DisplayName { get; set; }

    public string? Description { get; set; }

    public string? HubViewDescription { get; set; }

    public int? ParentTypeId { get; set; }

    public int? TypeGroupingId { get; set; }

    public TypeEntity? Parent { get; set; }

    public virtual TypeGroupingEntity? TypeGrouping { get; set; }

    public virtual ICollection<TaskEntity> Tasks { get; } = [];

    public virtual ICollection<TypeEntity> Children { get; } = [];
}
