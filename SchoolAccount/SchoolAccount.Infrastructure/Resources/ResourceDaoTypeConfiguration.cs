using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolAccount.Application.Persistence.Resources;

namespace SchoolAccount.Infrastructure.Resources;

internal class ResourceDaoTypeConfiguration : IEntityTypeConfiguration<ResourceDao>
{
    public void Configure(EntityTypeBuilder<ResourceDao> builder)
    {
        builder.ToTable("Resource", "dbo").HasKey(x => x.Id);
    }
}