using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolAccount.Domain.Entities;
using SchoolAccount.Infrastructure.Configuration.Constants;

namespace SchoolAccount.Infrastructure.Configuration;

public class SchoolTypeEntityConfiguration : IEntityTypeConfiguration<SchoolTypeEntity>
{
    public void Configure(EntityTypeBuilder<SchoolTypeEntity> builder)
    {
        builder.HasKey(e => e.Id).HasName(KeyConstants.Primary.SchoolType);

        builder.ToTable(TableConstants.Reference.SchoolType, SchemaConstants.Reference);

        builder.Property(e => e.Id).ValueGeneratedNever();
        builder.Property(e => e.Name).HasMaxLength(250);
    }
}
