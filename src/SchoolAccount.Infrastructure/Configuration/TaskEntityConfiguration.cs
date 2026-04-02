using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolAccount.Domain;
using SchoolAccount.Domain.Entities;
using SchoolAccount.Infrastructure.Configuration.Constants;

namespace SchoolAccount.Infrastructure.Configuration;

public sealed class TaskEntityConfiguration : IEntityTypeConfiguration<TaskEntity>
{
    private static class ColumnNames
    {
        public const string ReferenceNo = "TaskReferenceNo";
        public const string Name = "TaskName";
        public const string Description = "TaskDescription";
    }

    public void Configure(EntityTypeBuilder<TaskEntity> builder)
    {
        builder.ToTable(TableConstants.Transactional.Task, SchemaConstants.Transactional).HasKey(x => x.Id);

        builder.Property(x => x.ReferenceNo).HasColumnName(ColumnNames.ReferenceNo).HasMaxLength(50);
        builder.Property(x => x.Name).HasColumnName(ColumnNames.Name).HasMaxLength(200);
        builder.Property(x => x.Description).HasColumnName(ColumnNames.Description).HasMaxLength(4000);
        builder.Property(x => x.PublishComment).HasMaxLength(2000);
        builder.Property(x => x.ArchiveComment).HasMaxLength(2000);
        builder.Property(e => e.CreatedBy).HasMaxLength(Lengths.CreatedUpdatedBy).IsRequired();
        builder.Property(e => e.DateCreated).IsRequired();
        builder.Property(e => e.UpdatedBy).HasMaxLength(Lengths.CreatedUpdatedBy);
        builder.Property(e => e.DateUpdated);

        builder.HasIndex(x => x.ReferenceNo);
        builder.HasIndex(x => x.Name);
        builder.HasIndex(x => new { x.IsDeleted, x.IsLatestVersion });

        builder
            .HasOne(d => d.WorkflowState)
            .WithMany(p => p.Tasks)
            .HasForeignKey(d => d.WorkflowStateId)
            .OnDelete(DeleteBehavior.ClientSetNull);
    }
}
