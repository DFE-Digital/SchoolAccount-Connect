using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Teams.GetById;

public class GetTeamByIdHandler : IQueryHandler<GetTeamById, TeamResponse>
{
    private readonly IReadStore _readStore;

    public GetTeamByIdHandler(IReadStore readStore)
    {
        _readStore = readStore;
    }
    
    public async Task<Result<TeamResponse>> Handle(GetTeamById query, CancellationToken cancellationToken)
    {
        return await _readStore.GetTeamById(query.Id, cancellationToken);
    }
}