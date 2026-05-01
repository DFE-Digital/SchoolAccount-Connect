using System.Diagnostics.CodeAnalysis;

namespace SchoolAccount.Domain.Resources;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
public abstract class ResourceSourceMappingEntity
{
    public long EntityId { get; set; }

    public long ResourceId { get; set; }

    public Source Source { get; set; }

    public int ResourceOrder { get; set; }
}
