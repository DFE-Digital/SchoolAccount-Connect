using SchoolAccount.Application.Teams.GetById;

namespace SchoolAccount.Application.Abstractions.Data;

public interface ITeamReadStore
{
    public Task<TeamResponse?> GetTeamById(long id, CancellationToken cancellationToken);
}