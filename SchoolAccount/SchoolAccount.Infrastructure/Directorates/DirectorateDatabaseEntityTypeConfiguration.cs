using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SchoolAccount.Infrastructure.Directorates;

public class DirectorateDatabaseEntityTypeConfiguration
    : IEntityTypeConfiguration<DirectorateDatabaseEntity>
{
    public void Configure(EntityTypeBuilder<DirectorateDatabaseEntity> builder)
    {
        builder.ToTable("Directorate", "refData").HasKey(x => x.Id);
    }
}
