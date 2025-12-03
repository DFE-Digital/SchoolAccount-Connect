using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SchoolAccount.Infrastructure.Directorates;

public class DirectorateDatabaseEntityTypeConfiguration
    : IEntityTypeConfiguration<DirectorateDatabaseEntity>
{
    public void Configure(EntityTypeBuilder<DirectorateDatabaseEntity> builder)
    {
        builder.ToTable("Directorate", "refData").HasKey(x => x.Id);

        builder.Property(x => x.Name).HasMaxLength(250);

        builder.Property(x => x.Description).HasMaxLength(1500);
    }
}
