using System.Diagnostics.CodeAnalysis;
using SchoolAccount.Domain.SchoolTypes;
using TaxonomyEntity = SchoolAccount.Domain.Taxonomies.TaxonomyEntity;

namespace SchoolAccount.Domain.Tags;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
public class TagEntity
{
    public long Id { get; init; }

    public int TaxonomyId { get; set; }

    public required string TagName { get; set; }

    public required string Name { get; set; }

    public string? DisplayName { get; set; }

    public string? Description { get; set; }

    public long? ParentTagId { get; set; }

    public virtual ICollection<TagsSourceMappingEntity> TagsSourceMappings { get; } = [];

    public virtual ICollection<SchoolTypeTagMappingEntity> SchoolTypeTagMappings { get; } = [];

    public virtual required TaxonomyEntity Taxonomy { get; set; }
}
