using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolAccount.Domain.Resources;
using SchoolAccount.Infrastructure.Configuration.Constants;

namespace SchoolAccount.Infrastructure.Configuration;

public sealed class ResourceSourceMappingEntityConfiguration : IEntityTypeConfiguration<ResourceSourceMappingEntity>
{
    public void Configure(EntityTypeBuilder<ResourceSourceMappingEntity> builder)
    {
        builder.ToTable(TableConstants.Mapping.Resource, SchemaConstants.Transactional);

        builder.HasKey(rsm => new
        {
            rsm.EntityId,
            rsm.ResourceId,
            rsm.Source,
        });

        builder.Property(rsm => rsm.Source).HasConversion<int>().HasColumnName("SourceId");

        builder
            .HasDiscriminator(rsm => rsm.Source)
            .HasValue<TypeResourceMappingEntity>(Source.Type)
            .HasValue<TaskResourceMappingEntity>(Source.Task)
            .HasValue<SubTaskResourceMappingEntity>(Source.Subtask);
    }
}
