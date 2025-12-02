using Microsoft.EntityFrameworkCore;
using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Teams.GetById;

public class GetTeamByIdHandler : IQueryHandler<GetTeamById, TeamResponse>
{
    private readonly IApplicationDbContext _context;

    public GetTeamByIdHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Result<TeamResponse>> Handle(GetTeamById query, CancellationToken cancellationToken)
    {
        var team = await _context.Teams
            .Where(t => t.Id == query.Id)
            .Select(team => new TeamResponse
            {
                Id = team.Id,
                Name = team.ServiceName,
                DirectorateName = team.Directorate != null ? team.Directorate.Name : string.Empty
            })
            .FirstOrDefaultAsync(cancellationToken);

        return team;
    }
}