namespace SchoolAccount.Domain.Entities;

public class TagEntity
{
    public long Id { get; init; }

    public int TaxonomyId { get; set; }

    public string TagName { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? DisplayName { get; set; }

    public string? Description { get; set; }

    public long? ParentTagId { get; set; }

    public virtual ICollection<TagsSourceMappingEntity> TagsSourceMappings { get; } = [];

    public virtual ICollection<SchoolTypeTagMappingEntity> SchoolTypeTagMappings { get; } = [];

    public virtual TaxonomyEntity Taxonomy { get; } = null!;
}
