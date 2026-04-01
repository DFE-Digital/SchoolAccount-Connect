using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolAccount.Domain.Entities;
using SchoolAccount.Infrastructure.Configuration.Constants;

namespace SchoolAccount.Infrastructure.Configuration;

public class TypeGroupingEntityConfiguration : IEntityTypeConfiguration<TypeGroupingEntity>
{
    public void Configure(EntityTypeBuilder<TypeGroupingEntity> builder)
    {
        builder.HasKey(e => e.Id);

        builder.ToTable(TableConstants.Reference.TypeGrouping, SchemaConstants.Reference);

        builder.HasIndex(e => e.Name).IsUnique();

        builder.Property(e => e.DisplayName).HasMaxLength(250);
        builder.Property(e => e.Name).HasMaxLength(250);
    }
}
