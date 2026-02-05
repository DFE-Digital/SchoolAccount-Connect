using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolAccount.Infrastructure.Models;

namespace SchoolAccount.Infrastructure.Configuration;

public sealed class SubTaskEntityConfiguration : ConfigurationBase<SubTaskEntity>
{
    public override void Configure(EntityTypeBuilder<SubTaskEntity> builder)
    {
        base.Configure(builder);

        builder.ToTable("SubTask", "dbo");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.SubTaskReferenceNo).HasMaxLength(50);
        builder.Property(x => x.SubTaskName).HasMaxLength(200);
        builder.Property(x => x.SubTaskDescription).HasMaxLength(4000);

        builder.Property(x => x.DigitalTaskLink).HasMaxLength(2000);

        builder.Property(x => x.Comment).HasMaxLength(2000);
        builder.Property(x => x.ArchiveComment).HasMaxLength(2000);

        builder.HasIndex(x => x.TaskId);
        builder.HasIndex(x => new { x.TaskId, x.IsDeleted });
    }
}