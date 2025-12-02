using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolAccount.Application.Persistence.Teams;


namespace SchoolAccount.Infrastructure.Teams;

internal class TeamDatabaseEntityTypeConfiguration : IEntityTypeConfiguration<TeamDatabaseEntity>
{
    public void Configure(EntityTypeBuilder<TeamDatabaseEntity> builder)
    {
        builder.ToTable("Services", "dbo").HasKey(x => x.Id);
        
        // builder
        //     .ToTable("Services")
        //     .HasOne(x => x.Resource)
        //     .WithMany(x => x.Teams)
        //     .HasForeignKey(x => x.ResourceId);
        //     
        // builder
        //     .Navigation(e => e.Resource)
        //     .UsePropertyAccessMode(PropertyAccessMode.Property);

    }
}