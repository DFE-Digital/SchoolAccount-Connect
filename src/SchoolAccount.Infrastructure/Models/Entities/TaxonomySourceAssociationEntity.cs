using SchoolAccount.Infrastructure.Models.Interfaces;

namespace SchoolAccount.Infrastructure.Models.Entities;

public class TaxonomySourceAssociationEntity : IDatabaseEntity
{
    public long Id { get; set; }

    public int SourceId { get; set; }

    public int TaxonomyId { get; set; }

    public bool? IsActive { get; set; }

    public virtual SourceEntity Source { get; set; } = null!;

    public virtual TaxonomyEntity Taxonomy { get; set; } = null!;
}
