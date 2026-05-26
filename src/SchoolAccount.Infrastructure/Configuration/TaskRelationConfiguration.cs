using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolAccount.Domain.Tasks;
using SchoolAccount.Infrastructure.Configuration.Constants;
using static SchoolAccount.Infrastructure.Configuration.Constants.TableConstants;

namespace SchoolAccount.Infrastructure.Configuration;

public sealed class TaskRelationConfiguration : IEntityTypeConfiguration<TaskRelationEntity>
{
    public void Configure(EntityTypeBuilder<TaskRelationEntity> builder)
    {
        builder.ToTable(Transactional.TaskRelation, SchemaConstants.Transactional).HasKey(x => x.Id);

        builder.Property(tr => tr.RelatedOrder);
    }
}
