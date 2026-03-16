using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolAccount.Infrastructure.Configuration.Constants;
using SchoolAccount.Infrastructure.Models;
using SchoolAccount.Infrastructure.Models.Entities;

namespace SchoolAccount.Infrastructure.Configuration;

public class TaxonomySourceAssociationEntityConfiguration : IEntityTypeConfiguration<TaxonomySourceAssociationEntity>
{
    public void Configure(EntityTypeBuilder<TaxonomySourceAssociationEntity> builder)
    {
        builder.HasKey(e => e.Id).HasName(KeyConstants.Primary.TaxonomySourceAssociation);

        builder.ToTable(TableConstants.Mapping.Taxonomy, SchemaConstants.Reference);

        builder.HasIndex(e => new { e.SourceId, e.TaxonomyId }, "UQ_TaxonomySource_Association").IsUnique();

        builder
            .HasOne(d => d.Source)
            .WithMany(p => p.TaxonomySourceAssociations)
            .HasForeignKey(d => d.SourceId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_TaxanomySourceAssociation.SourceId");

        builder
            .HasOne(d => d.Taxonomy)
            .WithMany(p => p.TaxonomySourceAssociations)
            .HasForeignKey(d => d.TaxonomyId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_TaxanomySourceAssociation.TaxonomyId");
    }
}
