using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolAccount.Infrastructure.Models;

namespace SchoolAccount.Infrastructure.Configuration;

public sealed class TaskEntityConfiguration : ConfigurationBase<TaskEntity>
{
    public override void Configure(EntityTypeBuilder<TaskEntity> builder)
    {
        base.Configure(builder);

        builder.ToTable("Task", "dbo");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.TaskReferenceNo).HasMaxLength(50);
        builder.Property(x => x.TaskName).HasMaxLength(200);
        builder.Property(x => x.TaskDescription).HasMaxLength(4000);

        builder.Property(x => x.PublishComment).HasMaxLength(2000);
        builder.Property(x => x.ArchiveComment).HasMaxLength(2000);

        builder.HasIndex(x => x.TaskReferenceNo);
        builder.HasIndex(x => x.TaskName);
        builder.HasIndex(x => new { x.IsDeleted, x.IsLatestVersion });
    }
}