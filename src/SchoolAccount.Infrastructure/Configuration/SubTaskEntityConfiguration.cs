using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolAccount.Domain.Common;
using SchoolAccount.Domain.Subtasks;
using SchoolAccount.Infrastructure.Configuration.Constants;

namespace SchoolAccount.Infrastructure.Configuration;

public sealed class SubTaskEntityConfiguration : IEntityTypeConfiguration<SubTaskEntity>
{
    private static class ColumnNames
    {
        public const string Description = "SubTaskDescription";
        public const string Name = "SubTaskName";
        public const string ReferenceNo = "SubTaskReferenceNo";
        public const string Requirement = "RequirementId";
        public const string WorkflowState = "WorkflowStateId";
    }

    public void Configure(EntityTypeBuilder<SubTaskEntity> builder)
    {
        builder.ToTable(TableConstants.Transactional.SubTask, SchemaConstants.Transactional).HasKey(x => x.Id);

        builder.Property(x => x.ReferenceNo).HasColumnName(ColumnNames.ReferenceNo).HasMaxLength(50);
        builder.Property(x => x.Name).HasColumnName(ColumnNames.Name).HasMaxLength(200);
        builder.Property(x => x.Description).HasColumnName(ColumnNames.Description).HasMaxLength(4000);
        builder.Property(x => x.DigitalTaskLink).HasMaxLength(2000);
        builder.Property(x => x.Comment).HasMaxLength(2000);
        builder.Property(x => x.ArchiveComment).HasMaxLength(2000);
        builder.HasIndex(x => x.TaskId);
        builder.HasIndex(x => new { x.TaskId, x.IsDeleted });
        builder.Property(x => x.Requirement).HasConversion<int>().HasColumnName(ColumnNames.Requirement);
        builder.Property(x => x.StartDate);
        builder.Property(x => x.StartDateIsExact);
        builder.Property(x => x.DueDate);
        builder.Property(x => x.DueDateIsExact);
        builder.Property(x => x.ExpiryDate);
        builder.Property(x => x.DisplayDate);
        builder.Property(x => x.WorkflowState).HasConversion<int>().HasColumnName(ColumnNames.WorkflowState);
        builder.Property(x => x.Version);
        builder.Property(e => e.CreatedBy).HasMaxLength(Lengths.CreatedUpdatedBy).IsRequired();
        builder.Property(e => e.DateCreated).IsRequired();
        builder.Property(e => e.UpdatedBy).HasMaxLength(Lengths.CreatedUpdatedBy);
        builder.Property(e => e.DateUpdated);

        builder
            .HasOne(d => d.Task)
            .WithMany(p => p.SubTasks)
            .HasForeignKey(d => d.TaskId)
            .OnDelete(DeleteBehavior.ClientSetNull);

        builder
            .HasMany(x => x.TagsSourceMappings)
            .WithOne(x => x.SubTask)
            .HasForeignKey(x => x.EntityId)
            .OnDelete(DeleteBehavior.ClientSetNull);
    }
}
