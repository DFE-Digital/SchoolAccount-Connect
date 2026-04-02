using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolAccount.Domain.Entities;
using SchoolAccount.Infrastructure.Configuration.Constants;

namespace SchoolAccount.Infrastructure.Configuration;

public class WorkflowStateEntityConfiguration : IEntityTypeConfiguration<WorkflowStateEntity>
{
    public void Configure(EntityTypeBuilder<WorkflowStateEntity> builder)
    {
        builder.HasKey(e => e.Id).HasName(KeyConstants.Primary.WorkflowState);

        builder.ToTable(TableConstants.Reference.WorkflowState, SchemaConstants.Reference);

        builder.Property(e => e.Id).ValueGeneratedNever();
        builder.Property(e => e.Description).HasMaxLength(1500);
        builder.Property(e => e.Name).HasMaxLength(250);
    }
}
