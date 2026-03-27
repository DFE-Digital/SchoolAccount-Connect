using SchoolAccount.Domain.Interfaces;

namespace SchoolAccount.Domain.Entities;

public class TaxonomyEntity : IIdNameEntity<int>
{
    public int Id { get; init; }

    public string Name { get; set; } = null!;

    public int? TaxonomyGroupingId { get; set; }

    public string? DisplayName { get; set; }

    public bool? IsMandatory { get; set; }

    public bool? IsMultiSelect { get; set; }

    public string? Description { get; set; }

    public string TaxonomyName { get; set; } = null!;

    public virtual ICollection<TagEntity> Tags { get; } = [];

    public virtual TaxonomyGroupingEntity? TaxonomyGrouping { get; set; }

    public virtual ICollection<TaxonomySourceAssociationEntity> TaxonomySourceAssociations { get; } = [];
}
