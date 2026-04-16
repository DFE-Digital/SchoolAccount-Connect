using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolAccount.Domain.Types;
using SchoolAccount.Infrastructure.Configuration.Constants;

namespace SchoolAccount.Infrastructure.Configuration;

public class TypeTaskMappingEntityConfiguration : IEntityTypeConfiguration<TypeTaskMappingEntity>
{
    public void Configure(EntityTypeBuilder<TypeTaskMappingEntity> builder)
    {
        builder.HasKey(e => e.Id);

        builder.ToTable(TableConstants.Mapping.Type, SchemaConstants.Transactional);

        builder.HasIndex(e => new { e.TaskId, e.TypeId }).IsUnique();

        builder
            .HasOne(d => d.Task)
            .WithMany(p => p.TypeTaskMappings)
            .HasForeignKey(d => d.TaskId)
            .OnDelete(DeleteBehavior.ClientSetNull);

        builder
            .HasOne(d => d.Type)
            .WithMany(p => p.TypeTaskMappings)
            .HasForeignKey(d => d.TypeId)
            .OnDelete(DeleteBehavior.ClientSetNull);
    }
}
