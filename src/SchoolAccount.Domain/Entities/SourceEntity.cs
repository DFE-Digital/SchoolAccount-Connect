namespace SchoolAccount.Domain.Entities;

public class SourceEntity
{
    public int Id { get; init; }

    public string Name { get; set; } = null!;

    public virtual ICollection<TagsSourceMappingEntity> TagsSourceMappings { get; } = [];

    public virtual ICollection<TaxonomySourceAssociationEntity> TaxonomySourceAssociations { get; } = [];
}
