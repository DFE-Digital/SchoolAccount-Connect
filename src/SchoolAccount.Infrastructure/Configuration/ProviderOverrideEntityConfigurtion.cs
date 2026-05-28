using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolAccount.Domain.Providers;
using SchoolAccount.Infrastructure.Configuration.Constants;

namespace SchoolAccount.Infrastructure.Configuration;

public sealed class ProviderOverrideEntityConfiguration : IEntityTypeConfiguration<ProviderOverrideEntity>
{
    private static class ColumnNames
    {
        public const string Id = "PrId";
        public const string UkPrn = "UKPRN";
        public const string HasAccess = "HasAccess";
        public const string SchoolType = "SchoolTypeId";
        public const string SchoolName = "ProviderName";
    }

    public void Configure(EntityTypeBuilder<ProviderOverrideEntity> builder)
    {
        builder
            .ToTable(TableConstants.Administrative.ProviderOverride, SchemaConstants.Transactional)
            .HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName(ColumnNames.Id);
        builder.Property(x => x.UkPrn).HasColumnName(ColumnNames.UkPrn).HasMaxLength(250).IsRequired();
        builder.Property(x => x.HasAccess).HasColumnName(ColumnNames.HasAccess).IsRequired();
        builder.Property(x => x.SchoolType).HasConversion<int>().HasColumnName(ColumnNames.SchoolType).IsRequired();
        builder
            .Property(x => x.SchoolName)
            .HasColumnName(ColumnNames.SchoolName)
            .HasMaxLength(int.MaxValue)
            .IsRequired();

        builder.HasIndex(x => x.Id);
    }
}
