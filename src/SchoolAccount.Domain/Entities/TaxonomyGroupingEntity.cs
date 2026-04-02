namespace SchoolAccount.Domain.Entities;

public class TaxonomyGroupingEntity
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? DisplayName { get; set; }

    public int? PageNo { get; set; }

    public virtual ICollection<TaxonomyEntity> Taxonomies { get; } = [];
}
