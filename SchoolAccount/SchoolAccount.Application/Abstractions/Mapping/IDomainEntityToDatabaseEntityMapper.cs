using SchoolAccount.Application.Abstractions.Models;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Abstractions.Mapping;

public interface IDomainEntityToDatabaseEntityMapper<in TDomainEntity, TDatabaseEntity>
    where TDomainEntity : Entity
    where TDatabaseEntity : IDatabaseEntity
{
    TDatabaseEntity Map(TDomainEntity source);

    void Map(TDomainEntity source, TDatabaseEntity destination);
}
