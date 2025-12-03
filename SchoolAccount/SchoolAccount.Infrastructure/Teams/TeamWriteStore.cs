using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Domain.Teams;

namespace SchoolAccount.Infrastructure.Teams;

internal class TeamWriteStore(ApplicationDbContext context) : ITeamWriteStore
{
    public async Task<long> CreateTeamAsync(Team team, CancellationToken token)
    {
        await context.Teams.AddAsync(team.MapToDatabaseEntity(), token);
        return await context.SaveChangesAsync(token);
    }
}