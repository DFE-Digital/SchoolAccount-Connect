using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolAccount.Domain.Taxonomies;
using SchoolAccount.Infrastructure.Configuration.Constants;

namespace SchoolAccount.Infrastructure.Configuration;

public class TaxonomySourceAssociationEntityConfiguration : IEntityTypeConfiguration<TaxonomySourceAssociationEntity>
{
    public void Configure(EntityTypeBuilder<TaxonomySourceAssociationEntity> builder)
    {
        builder.HasKey(e => e.Id).HasName(KeyConstants.Primary.TaxonomySourceAssociation);

        builder.ToTable(TableConstants.Mapping.Taxonomy, SchemaConstants.Reference);

        builder
            .HasOne(d => d.Source)
            .WithMany(p => p.TaxonomySourceAssociations)
            .HasForeignKey(d => d.SourceId)
            .OnDelete(DeleteBehavior.ClientSetNull);

        builder
            .HasOne(d => d.Taxonomy)
            .WithMany(p => p.TaxonomySourceAssociations)
            .HasForeignKey(d => d.TaxonomyId)
            .OnDelete(DeleteBehavior.ClientSetNull);
    }
}
