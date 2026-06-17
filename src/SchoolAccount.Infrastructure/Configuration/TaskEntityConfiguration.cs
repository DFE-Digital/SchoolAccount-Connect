using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolAccount.Domain.Common;
using SchoolAccount.Domain.Resources;
using SchoolAccount.Domain.Tasks;
using SchoolAccount.Domain.Types;
using SchoolAccount.Infrastructure.Configuration.Constants;
using static SchoolAccount.Infrastructure.Configuration.Constants.TableConstants;

namespace SchoolAccount.Infrastructure.Configuration;

public sealed class TaskEntityConfiguration : IEntityTypeConfiguration<TaskEntity>
{
    private static class ColumnNames
    {
        public const string Description = "TaskDescription";
        public const string Name = "TaskName";
        public const string ReferenceNo = "TaskReferenceNo";
        public const string Requirement = "RequirementId";
        public const string Source = "SourceId";
        public const string WorkflowState = "WorkflowStateId";
    }

    public void Configure(EntityTypeBuilder<TaskEntity> builder)
    {
        builder.ToTable(Transactional.Task, SchemaConstants.Transactional).HasKey(x => x.Id);

        builder.Property(x => x.ReferenceNo).HasColumnName(ColumnNames.ReferenceNo).HasMaxLength(50);
        builder.Property(x => x.Name).HasColumnName(ColumnNames.Name).HasMaxLength(200);
        builder.Property(x => x.Description).HasColumnName(ColumnNames.Description).HasMaxLength(4000);
        builder.Property(x => x.PublishComment).HasMaxLength(2000);
        builder.Property(x => x.ArchiveComment).HasMaxLength(2000);
        builder.Property(x => x.Requirement).HasConversion<int>().HasColumnName(ColumnNames.Requirement);
        builder.Property(x => x.WorkflowState).HasConversion<int>().HasColumnName(ColumnNames.WorkflowState);
        builder.Property(e => e.CreatedBy).HasMaxLength(Lengths.CreatedUpdatedBy).IsRequired();
        builder.Property(e => e.DateCreated).IsRequired();
        builder.Property(e => e.UpdatedBy).HasMaxLength(Lengths.CreatedUpdatedBy);
        builder.Property(e => e.DateUpdated);

        builder.HasIndex(x => x.ReferenceNo);
        builder.HasIndex(x => x.Name);
        builder.HasIndex(x => new { x.IsDeleted, x.IsLatestVersion });

        builder
            .HasMany(t => t.Resources)
            .WithMany()
            .UsingEntity<TaskResourceMappingEntity>(
                j => j.HasOne<ResourceEntity>().WithMany().HasForeignKey(rsm => rsm.ResourceId),
                j => j.HasOne<TaskEntity>().WithMany().HasForeignKey(rsm => rsm.EntityId)
            );

        builder
            .HasMany(t => t.RelatedTasks)
            .WithMany()
            .UsingEntity<TaskRelationEntity>(
                l => l.HasOne<TaskEntity>().WithMany().HasForeignKey(tr => tr.RelatedTaskId),
                r => r.HasOne<TaskEntity>().WithMany().HasForeignKey(tr => tr.TaskId)
            );

        builder
            .HasMany(t => t.Types)
            .WithMany(p => p.Tasks)
            .UsingEntity<TypeTaskMappingEntity>(
                j => j.HasOne<TypeEntity>().WithMany().HasForeignKey(ttm => ttm.TypeId),
                j => j.HasOne<TaskEntity>().WithMany().HasForeignKey(ttm => ttm.TaskId),
                j =>
                {
                    j.HasKey(e => e.Id);
                    j.Property(e => e.TypeId).HasColumnName("TypeId");
                    j.Property(e => e.TaskId).HasColumnName("TaskId");
                    j.ToTable(Mapping.Type, SchemaConstants.Transactional);
                }
            );
    }
}
