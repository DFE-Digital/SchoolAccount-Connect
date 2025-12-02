using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolAccount.Application.Persistence.Resources;

namespace SchoolAccount.Infrastructure.Resources;

internal class ResourceDatabaseEntityTypeConfiguration : IEntityTypeConfiguration<ResourceDatabaseEntity>
{
    public void Configure(EntityTypeBuilder<ResourceDatabaseEntity> builder)
    {
        builder.ToTable("Resource", "dbo").HasKey(x => x.Id);
    }
}