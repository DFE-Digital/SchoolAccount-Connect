using SchoolAccount.Domain.Teams;

namespace SchoolAccount.Infrastructure.Teams;

internal static class TeamDomainExtensions
{
    internal static TeamDatabaseEntity MapToDatabaseEntity(this Team team)
    {
        return new TeamDatabaseEntity
        {
            Id = team.Id,
            ServiceName = team.Name,
            Acronym = team.Acronym,
            ServiceDescription = team.Description,
            ServiceStatusId = team.ServiceStatus.Id
        };
    }

    internal static Team MapToDomainEntity(this TeamDatabaseEntity teamEntity)
    {
        return new Team()
        {

        };
    }
}