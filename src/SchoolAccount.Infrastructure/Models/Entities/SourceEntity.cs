using SchoolAccount.Infrastructure.Models.Interfaces;

namespace SchoolAccount.Infrastructure.Models.Entities;

public class SourceEntity : IIdNameEntity<int>
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    //public virtual ICollection<ResourceSourceMapping> ResourceSourceMappings { get; set; } = new List<ResourceSourceMapping>();

    public virtual ICollection<TagsSourceMappingEntity> TagsSourceMappings { get; } = [];

    public virtual ICollection<TaxonomySourceAssociationEntity> TaxonomySourceAssociations { get; } = [];
}
