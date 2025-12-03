using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Teams.GetById;

public class GetTeamByIdHandler : IQueryHandler<GetTeamById, TeamResponse>
{
    private readonly ITeamReadStore _teamReadStore;

    public GetTeamByIdHandler(ITeamReadStore teamReadStore)
    {
        _teamReadStore = teamReadStore;
    }

    public async Task<Result<TeamResponse>> Handle(
        GetTeamById query,
        CancellationToken cancellationToken
    )
    {
        return await _teamReadStore.GetTeamById(query.Id, cancellationToken);
    }
}
