using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolAccount.Application.Persistence.Teams;


namespace SchoolAccount.Infrastructure.Teams;

internal class TeamDaoTypeConfiguration : IEntityTypeConfiguration<TeamDao>
{
    public void Configure(EntityTypeBuilder<TeamDao> builder)
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