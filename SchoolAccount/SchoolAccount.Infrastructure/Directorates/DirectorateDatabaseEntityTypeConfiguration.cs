using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolAccount.Application.Persistence.Directorates;

namespace SchoolAccount.Infrastructure.Directorates;

public class DirectorateDatabaseEntityTypeConfiguration : IEntityTypeConfiguration<DirectorateDatabaseEntity>
{
    public void Configure(EntityTypeBuilder<DirectorateDatabaseEntity> builder)
    {
        builder
            .ToTable("Directorate", "refData")
            .HasKey(x => x.Id);
    }
}