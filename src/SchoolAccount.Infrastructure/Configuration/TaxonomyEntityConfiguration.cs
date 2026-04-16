using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolAccount.Infrastructure.Configuration.Constants;
using TaxonomyEntity = SchoolAccount.Domain.Taxonomies.TaxonomyEntity;

namespace SchoolAccount.Infrastructure.Configuration;

public class TaxonomyEntityConfiguration : IEntityTypeConfiguration<TaxonomyEntity>
{
    public void Configure(EntityTypeBuilder<TaxonomyEntity> builder)
    {
        builder.HasKey(e => e.Id).HasName(KeyConstants.Primary.Taxonomy);

        builder.ToTable(TableConstants.Reference.Taxonomy, SchemaConstants.Reference);

        builder.Property(e => e.Description).HasMaxLength(2500);
        builder.Property(e => e.DisplayName).HasMaxLength(1500);
        builder.Property(e => e.Name).HasMaxLength(250);
        builder.Property(e => e.TaxonomyName).HasMaxLength(250);

        builder.HasOne(d => d.TaxonomyGrouping).WithMany(p => p.Taxonomies).HasForeignKey(d => d.TaxonomyGroupingId);
    }
}
