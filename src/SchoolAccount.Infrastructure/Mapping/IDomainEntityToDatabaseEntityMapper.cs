using SchoolAccount.Infrastructure.Models.Interfaces;
using SchoolAccount.Kernel;

namespace SchoolAccount.Infrastructure.Mapping;

public interface IDomainEntityToDatabaseEntityMapper<in TDomainEntity, TDatabaseEntity>
    where TDomainEntity : Entity
    where TDatabaseEntity : IDatabaseEntity
{
    TDatabaseEntity Map(TDomainEntity source);

    void Map(TDomainEntity source, TDatabaseEntity destination);
}
