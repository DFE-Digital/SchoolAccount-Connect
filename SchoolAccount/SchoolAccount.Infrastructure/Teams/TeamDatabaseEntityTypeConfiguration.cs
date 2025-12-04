using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolAccount.Infrastructure.Configuration;

namespace SchoolAccount.Infrastructure.Teams;

internal sealed class TeamDatabaseEntityTypeConfiguration : ConfigurationBase<TeamDatabaseEntity>
{
    public override void Configure(EntityTypeBuilder<TeamDatabaseEntity> builder)
    {
        base.Configure(builder);

        builder.ToTable("Services", "dbo").HasKey(x => x.Id);

        builder.HasOne(x => x.Directorate).WithMany(x => x.Teams).HasForeignKey(x => x.DirectorateId);
        builder.HasOne(t => t.ServiceStatus).WithMany().HasForeignKey(t => t.ServiceStatusId).IsRequired();

        builder.Property(x => x.ServiceName).HasMaxLength(250);
        builder.Property(x => x.Acronym).HasMaxLength(50);
        builder.Property(x => x.ServiceDescription).HasMaxLength(1500);
        builder.Property(x => x.DigitalServiceLink).HasMaxLength(1000);
    }
}
