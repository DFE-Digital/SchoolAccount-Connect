using SchoolAccount.Domain.Teams;

namespace SchoolAccount.Application.Abstractions.Data;

public interface ITeamWriteStore
{
    Task<long> CreateTeamAsync(Team team, CancellationToken token);
}
