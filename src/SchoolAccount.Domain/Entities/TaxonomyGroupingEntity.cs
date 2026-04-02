using System.Diagnostics.CodeAnalysis;

namespace SchoolAccount.Domain.Entities;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
public class TaxonomyGroupingEntity
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public string? DisplayName { get; set; }

    public int? PageNo { get; set; }

    public virtual ICollection<TaxonomyEntity> Taxonomies { get; } = [];
}
