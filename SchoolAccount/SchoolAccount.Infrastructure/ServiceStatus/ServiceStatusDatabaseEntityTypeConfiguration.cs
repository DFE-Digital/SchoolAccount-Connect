using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SchoolAccount.Infrastructure.ServiceStatus;

public class ServiceStatusDatabaseEntityTypeConfiguration : IEntityTypeConfiguration<ServiceStatusDatabaseEntity>
{
    public void Configure(EntityTypeBuilder<ServiceStatusDatabaseEntity> builder)
    {
        builder
            .ToTable("ServiceStatus", "refData");

        builder
            .HasKey(x => x.Id);
    }
}