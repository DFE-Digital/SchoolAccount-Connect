using Microsoft.AspNetCore.Mvc;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Teams.GetById;

namespace SchoolAccount.Web.Manage.Controllers;

[Route("[controller]")]
internal sealed class TeamController(IQueryHandler<GetTeamById, TeamResponse> getTeamById)
    : Controller
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var query = new GetTeamById { Id = id };
        var team = await getTeamById.Handle(query, cancellationToken);

        return Json(team);
    }
}
