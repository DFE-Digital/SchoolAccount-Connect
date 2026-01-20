using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Features.Dashboard.Queries.GetDashboard;

namespace SchoolAccount.Web.Connect.Controllers;

[Authorize]
[Route("[controller]")]
public class DashboardController(IQueryHandler<GetDashboardQuery, DashboardLists> handler) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> GetAsync(CancellationToken token)
    {
        var result = await handler.Handle(new GetDashboardQuery(), token);
        
        //TODO: Pattern for handling failures
        return result.IsSuccess ? View("V2", result.Value) : NotFound();
    }
}