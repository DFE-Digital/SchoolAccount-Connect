using SchoolAccount.Domain.Interfaces;

namespace SchoolAccount.Domain.Entities;

public class SourceEntity : IIdNameEntity<int>
{
    public int Id { get; init; }

    public string Name { get; set; } = null!;

    //public virtual ICollection<ResourceSourceMapping> ResourceSourceMappings { get; set; } = new List<ResourceSourceMapping>();

    public virtual ICollection<TagsSourceMappingEntity> TagsSourceMappings { get; } = [];

    public virtual ICollection<TaxonomySourceAssociationEntity> TaxonomySourceAssociations { get; } = [];
}
