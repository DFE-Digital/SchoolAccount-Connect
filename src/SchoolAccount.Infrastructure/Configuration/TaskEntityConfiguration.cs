using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolAccount.Infrastructure.Configuration.Common;
using SchoolAccount.Infrastructure.Configuration.Constants;
using SchoolAccount.Infrastructure.Models.Entities;

namespace SchoolAccount.Infrastructure.Configuration;

public sealed class TaskEntityConfiguration : ConfigurationBase<TaskEntity>
{
    private static class ColumnNames
    {
        public const string ReferenceNo = "TaskReferenceNo";
        public const string Name = "TaskName";
        public const string Description = "TaskDescription";
    }
    
    public override void Configure(EntityTypeBuilder<TaskEntity> builder)
    {
        base.Configure(builder);

        builder
            .ToTable(
                TableConstants.Transactional.Task,
                SchemaConstants.Transactional)
            .HasKey(x => x.Id);

        builder
            .Property(x => x.ReferenceNo)
            .HasColumnName(ColumnNames.ReferenceNo)
            .HasMaxLength(50);
        builder
            .Property(x => x.Name)
            .HasColumnName(ColumnNames.Name)
            .HasMaxLength(200);
        builder
            .Property(x => x.Description)
            .HasColumnName(ColumnNames.Description)
            .HasMaxLength(4000);
        builder
            .Property(x => x.PublishComment)
            .HasMaxLength(2000);
        builder
            .Property(x => x.ArchiveComment)
            .HasMaxLength(2000);

        builder
            .HasIndex(x => x.ReferenceNo);
        builder
            .HasIndex(x => x.Name);
        builder
            .HasIndex(x => new { x.IsDeleted, x.IsLatestVersion });
    }
}