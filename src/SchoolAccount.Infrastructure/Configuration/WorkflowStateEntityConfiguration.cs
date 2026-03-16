using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolAccount.Infrastructure.Configuration.Constants;
using SchoolAccount.Infrastructure.Models;
using SchoolAccount.Infrastructure.Models.Entities;

namespace SchoolAccount.Infrastructure.Configuration;

public class WorkflowStateEntityConfiguration : IEntityTypeConfiguration<WorkflowStateEntity>
{
    public void Configure(EntityTypeBuilder<WorkflowStateEntity> builder)
    {
        builder.HasKey(e => e.Id).HasName(KeyConstants.Primary.WorkflowState);

        builder.ToTable(TableConstants.Reference.WorkflowState, SchemaConstants.Reference);

        builder.HasIndex(e => e.Name, "UQ__Workflow__737584F6CD1ECE89").IsUnique();

        builder.Property(e => e.Id).ValueGeneratedNever();
        builder.Property(e => e.Description).HasMaxLength(1500);
        builder.Property(e => e.Name).HasMaxLength(250);
    }
}
