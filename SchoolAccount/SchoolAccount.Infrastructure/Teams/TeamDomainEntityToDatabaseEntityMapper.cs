using SchoolAccount.Application.Abstractions.Mapping;
using SchoolAccount.Domain.Teams;

namespace SchoolAccount.Infrastructure.Teams;

public class TeamDomainEntityToDatabaseEntityMapper : IDomainEntityToDatabaseEntityMapper<Team, TeamDatabaseEntity>
{
    public TeamDatabaseEntity Map(Team source)
    {
        return new TeamDatabaseEntity
        {
            Id = source.Id,
            ServiceName = source.Name,
            Acronym = source.Acronym,
            ServiceDescription = source.Description,
            ServiceStatusId = source.ServiceStatus.Id,
        };
    }

    public void Map(Team source, TeamDatabaseEntity destination)
    {
        throw new NotImplementedException();
    }
}
