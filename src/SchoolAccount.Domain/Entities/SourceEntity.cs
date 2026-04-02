using System.Diagnostics.CodeAnalysis;

namespace SchoolAccount.Domain.Entities;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
public class SourceEntity
{
    public int Id { get; init; }

    public required string Name { get; set; }

    public virtual ICollection<TagsSourceMappingEntity> TagsSourceMappings { get; } = [];

    public virtual ICollection<TaxonomySourceAssociationEntity> TaxonomySourceAssociations { get; } = [];
}
