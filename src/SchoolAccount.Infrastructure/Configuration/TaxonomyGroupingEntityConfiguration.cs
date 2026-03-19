using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolAccount.Infrastructure.Configuration.Constants;
using SchoolAccount.Infrastructure.Models.Entities;

namespace SchoolAccount.Infrastructure.Configuration;

public class TaxonomyGroupingEntityConfiguration : IEntityTypeConfiguration<TaxonomyGroupingEntity>
{
    public void Configure(EntityTypeBuilder<TaxonomyGroupingEntity> builder)
    {
        builder.HasKey(e => e.Id).HasName(KeyConstants.Primary.TaxonomyGrouping);

        builder.ToTable(TableConstants.Reference.TaxonomyGrouping, SchemaConstants.Reference);

        builder.Property(e => e.DisplayName).HasMaxLength(1500);
        builder.Property(e => e.Name).HasMaxLength(250);
    }
}
