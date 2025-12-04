using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Domain.Teams;
using SchoolAccount.Infrastructure.Mapping;

namespace SchoolAccount.Infrastructure.Teams;

internal sealed class TeamWriteStore(
    ApplicationDbContext context,
    IDomainEntityToDatabaseEntityMapper<Team, TeamDatabaseEntity> mapper
) : ITeamWriteStore
{
    public async Task<long> CreateTeamAsync(Team team, CancellationToken token)
    {
        var teamDatabaseEntity = mapper.Map(team);

        await context.Teams.AddAsync(teamDatabaseEntity, token);

        return await context.SaveChangesAsync(token);
    }
}
