using System.Diagnostics.CodeAnalysis;

namespace SchoolAccount.Domain.Entities;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
public class TaxonomySourceAssociationEntity
{
    public long Id { get; set; }

    public int SourceId { get; set; }

    public int TaxonomyId { get; set; }

    public bool? IsActive { get; set; }

    public virtual required SourceEntity Source { get; set; }

    public virtual required TaxonomyEntity Taxonomy { get; set; }
}
