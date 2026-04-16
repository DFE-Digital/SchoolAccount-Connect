using System.Diagnostics.CodeAnalysis;

namespace SchoolAccount.Domain.Types;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
public class TypeGroupingEntity
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public required string DisplayName { get; set; }

    public int? TypeLevel { get; set; }

    public bool? IsMandatory { get; set; }

    public bool? IsMultiSelect { get; set; }

    public virtual ICollection<TypeEntity> Types { get; } = [];
}
