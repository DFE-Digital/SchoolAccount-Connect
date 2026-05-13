using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolAccount.Domain.Resources;
using SchoolAccount.Domain.Tasks;
using SchoolAccount.Domain.Types;
using SchoolAccount.Infrastructure.Configuration.Constants;

namespace SchoolAccount.Infrastructure.Configuration;

public class TypeEntityConfiguration : IEntityTypeConfiguration<TypeEntity>
{
    public void Configure(EntityTypeBuilder<TypeEntity> builder)
    {
        builder.HasKey(e => e.Id);

        builder.ToTable(TableConstants.Reference.Type, SchemaConstants.Reference);

        builder.HasIndex(e => e.Name).IsUnique();

        builder.HasIndex(e => e.TagName).IsUnique();

        builder.Property(e => e.Description).HasMaxLength(1500);

        builder.Property(e => e.DisplayName).HasMaxLength(250);

        builder.Property(e => e.Name).HasMaxLength(250);

        builder.Property(e => e.TagName).HasMaxLength(250);

        builder.HasOne(d => d.TypeGrouping).WithMany(p => p.Types).HasForeignKey(d => d.TypeGroupingId);

        builder.HasMany(x => x.Children).WithOne(x => x.Parent).HasForeignKey(x => x.ParentTypeId);
        
        builder
            .HasMany(t => t.Resources)
            .WithMany()
            .UsingEntity<TypeResourceMappingEntity>(
                j => j.HasOne<ResourceEntity>().WithMany().HasForeignKey(rsm => rsm.ResourceId),
                j => j.HasOne<TypeEntity>().WithMany().HasForeignKey(rsm => (int)rsm.EntityId)
            );
    }
}
