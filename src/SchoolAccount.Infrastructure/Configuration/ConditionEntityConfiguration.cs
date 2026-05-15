using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolAccount.Domain.Conditions;
using SchoolAccount.Domain.Subtasks;
using SchoolAccount.Infrastructure.Configuration.Constants;

namespace SchoolAccount.Infrastructure.Configuration;

public class ConditionEntityConfiguration: IEntityTypeConfiguration<ConditionEntity>
{
    public void Configure(EntityTypeBuilder<ConditionEntity> builder)
    {
        builder.ToTable("Condition", SchemaConstants.Reference).HasKey(x => x.Id);
        
        builder
            .HasMany(d => d.SubTaskConditions)
            .WithOne(p => p.Condition)
            .HasForeignKey(d => d.SubTaskId)
            .OnDelete(DeleteBehavior.ClientSetNull);
    }
}