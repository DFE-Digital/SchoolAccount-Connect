using Microsoft.EntityFrameworkCore;
using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Application.Teams.GetById;

namespace SchoolAccount.Infrastructure.Teams;

internal class ReadStore : IReadStore
{
    private readonly ApplicationDbContext _context;

    public ReadStore(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<TeamResponse?> GetTeamById(long id, CancellationToken cancellationToken)
    {
        return await _context.Teams
            .Where(t => t.Id == id)
            .Select(team => new TeamResponse
            {
                Id = team.Id,
                Name = team.ServiceName,
                DirectorateName = team.Directorate != null ? team.Directorate.Name : string.Empty
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}