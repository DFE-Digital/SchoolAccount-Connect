using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolAccount.Infrastructure.Configuration.Constants;
using SchoolAccount.Infrastructure.Models;
using SchoolAccount.Infrastructure.Models.Entities;

namespace SchoolAccount.Infrastructure.Configuration;

public class TagsSourceMappingEntityConfiguration : IEntityTypeConfiguration<TagsSourceMappingEntity>
{
    public void Configure(EntityTypeBuilder<TagsSourceMappingEntity> builder)
    {
        builder.HasKey(e => e.Id).HasName(KeyConstants.Primary.TagsSourceMapping);

        builder.ToTable(TableConstants.Mapping.Tag, SchemaConstants.Transactional);

        builder
            .HasIndex(
                e => new
                {
                    e.EntityId,
                    e.SourceId,
                    e.TagId,
                },
                "UQ_TagsSourceMapping_All"
            )
            .IsUnique();

        builder
            .HasOne(d => d.Source)
            .WithMany(p => p.TagsSourceMappings)
            .HasForeignKey(d => d.SourceId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_TagsSourceMapping .SourceId");

        builder
            .HasOne(d => d.Tag)
            .WithMany(p => p.TagsSourceMappings)
            .HasForeignKey(d => d.TagId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_TagsSourceMapping .TagId");
    }
}
