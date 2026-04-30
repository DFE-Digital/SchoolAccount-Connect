using System.Diagnostics.CodeAnalysis;

namespace SchoolAccount.Domain.Resources;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
public class ResourceEntity
{
    public long Id { get; set; }

    public required string ResourceName { get; set; }

    public string? DigitalLink { get; set; }
}
