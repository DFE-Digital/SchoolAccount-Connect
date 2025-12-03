using Microsoft.AspNetCore.Mvc;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Teams.CreateTeam;
using SchoolAccount.Application.Teams.GetById;

namespace SchoolAccount.Web.Manage.Controllers;

[Route("[controller]")]
public class TeamController(
    IQueryHandler<GetTeamById, TeamResponse> getTeamById,
    ICommandHandler<CreateTeamCommand, long> createTeamCommandHandler
) : Controller
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var query = new GetTeamById { Id = id };
        var team = await getTeamById.Handle(query, cancellationToken);

        return Json(team);
    }
    
    [HttpPost("")]
    public async Task<IActionResult> Create ([FromBody]CreateTeamCommand command)
    {
        var id = await createTeamCommandHandler.Handle(command, CancellationToken.None);
        return Created(new Uri(""), id);
    }
}