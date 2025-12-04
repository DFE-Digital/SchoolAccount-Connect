using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolAccount.Application.Models;
using SchoolAccount.Infrastructure.Models;

namespace SchoolAccount.Infrastructure.Configuration;

public abstract class ConfigurationBase<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : AuditableDatabaseEntity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.Property(e => e.CreatedBy).HasMaxLength(Lengths.CreatedUpdatedBy).IsRequired();
        builder.Property(e => e.DateCreated).IsRequired();
        builder.Property(e => e.UpdatedBy).HasMaxLength(Lengths.CreatedUpdatedBy);
        builder.Property(e => e.DateUpdated);
    }
}
