using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolAccount.Domain.Resources;
using SchoolAccount.Infrastructure.Configuration.Constants;
using static SchoolAccount.Infrastructure.Configuration.Constants.TableConstants;

namespace SchoolAccount.Infrastructure.Configuration;

public sealed class ResourceEntityConfiguration : IEntityTypeConfiguration<ResourceEntity>
{
    public void Configure(EntityTypeBuilder<ResourceEntity> builder)
    {
        builder.ToTable(Transactional.Resource, SchemaConstants.Transactional);

        builder.Property(x => x.ResourceName);
        builder.Property(x => x.DigitalLink);
    }
}
