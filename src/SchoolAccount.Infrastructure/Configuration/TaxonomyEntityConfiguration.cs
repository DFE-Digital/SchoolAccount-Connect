using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolAccount.Infrastructure.Configuration.Constants;
using SchoolAccount.Infrastructure.Models;
using SchoolAccount.Infrastructure.Models.Entities;

namespace SchoolAccount.Infrastructure.Configuration;

public class TaxonomyEntityConfiguration : IEntityTypeConfiguration<TaxonomyEntity>
{
    public void Configure(EntityTypeBuilder<TaxonomyEntity> builder)
    {
        builder
            .HasKey(e => e.Id)
            .HasName(KeyConstants.Primary.Taxonomy);

        builder
            .ToTable(
                TableConstants.Reference.Taxonomy,
                SchemaConstants.Reference);

        builder
            .HasIndex(e => e.Name, "UQ_Taxonomy_Name")
            .IsUnique();

        builder
            .HasIndex(e => e.TaxonomyName, "UQ_Taxonomy_TaxonomyName")
            .IsUnique();

        builder
            .Property(e => e.Description)
            .HasMaxLength(2500);
        builder
            .Property(e => e.DisplayName)
            .HasMaxLength(1500);
        builder
            .Property(e => e.Name)
            .HasMaxLength(250);
        builder
            .Property(e => e.TaxonomyName)
            .HasMaxLength(250);

        builder
            .HasOne(d => d.TaxonomyGrouping)
            .WithMany(p => p.Taxonomies)
            .HasForeignKey(d => d.TaxonomyGroupingId)
            .HasConstraintName("FK_Taxonomy.TaxonomyGroupingId");
    }
}