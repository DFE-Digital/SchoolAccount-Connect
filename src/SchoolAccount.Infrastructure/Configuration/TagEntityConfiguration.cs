using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolAccount.Domain.Tags;
using SchoolAccount.Infrastructure.Configuration.Constants;

namespace SchoolAccount.Infrastructure.Configuration;

public class TagEntityConfiguration : IEntityTypeConfiguration<TagEntity>
{
    public void Configure(EntityTypeBuilder<TagEntity> builder)
    {
        builder.HasKey(e => e.Id).HasName(KeyConstants.Primary.Tag);

        builder.ToTable(TableConstants.Reference.Tag, SchemaConstants.Reference);

        builder.Property(e => e.Description).HasMaxLength(1500);
        builder.Property(e => e.DisplayName).HasMaxLength(250);
        builder.Property(e => e.Name).HasMaxLength(250);
        builder.Property(e => e.TagName).HasMaxLength(250);

        builder
            .HasOne(d => d.Taxonomy)
            .WithMany(p => p.Tags)
            .HasForeignKey(d => d.TaxonomyId)
            .OnDelete(DeleteBehavior.ClientSetNull);
    }
}
