using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolAccount.Infrastructure.Configuration.Constants;
using SchoolAccount.Infrastructure.Models;
using SchoolAccount.Infrastructure.Models.Entities;

namespace SchoolAccount.Infrastructure.Configuration;

public class SchoolTypeTagMappingEntityConfiguration : IEntityTypeConfiguration<SchoolTypeTagMappingEntity>
{
    public void Configure(EntityTypeBuilder<SchoolTypeTagMappingEntity> builder)
    {
        builder.HasKey(e => e.Id).HasName(KeyConstants.Primary.SchoolTypeTagMapping);

        builder.ToTable(TableConstants.Mapping.SchoolType, SchemaConstants.Reference);

        builder
            .HasOne(d => d.SchoolType)
            .WithMany(p => p.SchoolTypeTagMappings)
            .HasForeignKey(d => d.SchoolTypeId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_SchoolTypeTagMapping.SchoolTypeId");

        builder
            .HasOne(x => x.Tag)
            .WithMany(x => x.SchoolTypeTagMappings)
            .HasForeignKey(x => x.TagId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName($"FK_SchoolTypeTagMapping.TagId");
    }
}
