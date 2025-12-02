using SchoolAccount.Application.Teams.GetById;

namespace SchoolAccount.Application.Abstractions.Data;

public interface IReadStore
{
    public Task<TeamResponse?> GetTeamById(long id, CancellationToken cancellationToken);
}