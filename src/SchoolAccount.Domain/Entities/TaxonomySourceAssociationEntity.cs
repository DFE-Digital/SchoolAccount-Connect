namespace SchoolAccount.Domain.Entities;

public class TaxonomySourceAssociationEntity
{
    public long Id { get; set; }

    public int SourceId { get; set; }

    public int TaxonomyId { get; set; }

    public bool? IsActive { get; set; }

    public virtual SourceEntity Source { get; set; } = null!;

    public virtual TaxonomyEntity Taxonomy { get; set; } = null!;
}
