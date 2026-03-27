using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolAccount.Domain.Entities;
using SchoolAccount.Infrastructure.Configuration.Constants;

namespace SchoolAccount.Infrastructure.Configuration;

public class TagsSourceMappingEntityConfiguration : IEntityTypeConfiguration<TagsSourceMappingEntity>
{
    public void Configure(EntityTypeBuilder<TagsSourceMappingEntity> builder)
    {
        builder.HasKey(e => e.Id).HasName(KeyConstants.Primary.TagsSourceMapping);

        builder.ToTable(TableConstants.Mapping.Tag, SchemaConstants.Transactional);

        builder
            .HasOne(d => d.Source)
            .WithMany(p => p.TagsSourceMappings)
            .HasForeignKey(d => d.SourceId)
            .OnDelete(DeleteBehavior.ClientSetNull);

        builder
            .HasOne(d => d.Tag)
            .WithMany(p => p.TagsSourceMappings)
            .HasForeignKey(d => d.TagId)
            .OnDelete(DeleteBehavior.ClientSetNull);
    }
}
