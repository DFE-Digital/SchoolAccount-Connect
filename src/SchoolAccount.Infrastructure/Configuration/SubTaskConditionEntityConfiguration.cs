using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolAccount.Domain.Subtasks;
using SchoolAccount.Infrastructure.Configuration.Constants;

namespace SchoolAccount.Infrastructure.Configuration;

public class SubTaskConditionEntityConfiguration: IEntityTypeConfiguration<SubTaskConditionEntity>
{
    public void Configure(EntityTypeBuilder<SubTaskConditionEntity> builder)
    {
        builder.ToTable("SubTaskCondition", SchemaConstants.Transactional).HasKey(x => x.Id);
        
        builder
            .HasOne(d => d.SubTask)
            .WithMany(p => p.Conditions)
            .HasForeignKey(d => d.SubTaskId)
            .OnDelete(DeleteBehavior.ClientSetNull);
    }
}