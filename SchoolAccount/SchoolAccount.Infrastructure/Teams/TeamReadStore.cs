using Microsoft.EntityFrameworkCore;
using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Application.Teams.GetById;
using SchoolAccount.Domain.Teams;

namespace SchoolAccount.Infrastructure.Teams;

internal class TeamReadStore(ApplicationDbContext context) : ITeamReadStore
{
    public async Task<TeamResponse?> GetTeamById(long id, CancellationToken cancellationToken)
    {
        return await context
            .Teams.AsNoTracking()
            .Where(t => t.Id == id)
            .Select(team => new TeamResponse //Used to select columns prior to in memory materialization
            {
                Id = team.Id,
                Name = team.ServiceName,
                DirectorateName = team.Directorate != null ? team.Directorate.Name : string.Empty,
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Team> GetTeamEntityById(long id, CancellationToken cancellationToken)
    {
        var teamEntity = await context
            .Teams.AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

        return teamEntity?.MapToDomainEntity() ?? null;
    }
}
