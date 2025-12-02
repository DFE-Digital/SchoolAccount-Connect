using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace SchoolAccount.Infrastructure.Teams;

internal class TeamDatabaseEntityTypeConfiguration : IEntityTypeConfiguration<TeamDatabaseEntity>
{
    public void Configure(EntityTypeBuilder<TeamDatabaseEntity> builder)
    {
        builder.ToTable("Services", "dbo").HasKey(x => x.Id);
        
        builder
            .HasOne(x => x.Directorate)
            .WithMany(x => x.Teams)
            .HasForeignKey(x => x.DirectorateId);

        // builder
        //     .Navigation(e => e.Directorate);
    }
}