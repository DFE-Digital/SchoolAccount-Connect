using System.Diagnostics.CodeAnalysis;
using SchoolAccount.Domain.Tags;

namespace SchoolAccount.Domain.Taxonomies;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
public partial class TaxonomyEntity
{
    public int Id { get; init; }

    public required string Name { get; set; }

    public int? TaxonomyGroupingId { get; set; }

    public string? DisplayName { get; set; }

    public bool? IsMandatory { get; set; }

    public bool? IsMultiSelect { get; set; }

    public string? Description { get; set; }

    public required string TaxonomyName { get; set; }

    public virtual TaxonomyGroupingEntity? TaxonomyGrouping { get; set; }

    public virtual ICollection<TagEntity> Tags { get; } = [];

    public virtual ICollection<TaxonomySourceAssociationEntity> TaxonomySourceAssociations { get; } = [];
}
