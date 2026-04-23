using System.Diagnostics.CodeAnalysis;
using SchoolAccount.Domain.Tags;
using SchoolAccount.Domain.Taxonomies;

namespace SchoolAccount.Domain.Sources;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
public class SourceEntity
{
    public int Id { get; init; }

    public required string Name { get; set; }

    public virtual ICollection<TagsSourceMappingEntity> TagsSourceMappings { get; } = [];

    public virtual ICollection<TaxonomySourceAssociationEntity> TaxonomySourceAssociations { get; } = [];
}
