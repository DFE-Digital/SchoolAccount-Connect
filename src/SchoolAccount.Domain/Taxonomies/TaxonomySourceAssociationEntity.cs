using System.Diagnostics.CodeAnalysis;
using SchoolAccount.Domain.Sources;

namespace SchoolAccount.Domain.Taxonomies;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
public class TaxonomySourceAssociationEntity
{
    public long Id { get; set; }

    public int SourceId { get; set; }

    public int TaxonomyId { get; set; }

    public bool? IsActive { get; set; }

    public virtual required SourceEntity Source { get; set; }

    public virtual required Taxonomies.TaxonomyEntity Taxonomy { get; set; }
}
