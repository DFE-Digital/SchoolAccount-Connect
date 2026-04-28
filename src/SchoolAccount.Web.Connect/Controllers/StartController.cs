using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolAccount.Kernel;
using SchoolAccount.Kernel.Organisations;
using SchoolAccount.Web.Connect.Authentication.Attributes;
using SchoolAccount.Web.Connect.Models;

namespace SchoolAccount.Web.Connect.Controllers;

public class StartController(IUserContext userContext) : Controller
{
    [AllowAnonymous]
    [HttpGet(RouteConstants.Start.Index)]
    public IActionResult Index()
    {
        return !userContext.IsAuthenticated 
            ? View() 
            : Redirect(RouteConstants.Root);
    }

    [Authorize]
    [HttpGet(RouteConstants.Start.MatAcceptance)]
    [RestrictOrganisationType(typeof(TrustOrganisation))]
    public IActionResult MatAcceptance([FromQuery] string? returnAddress)
    {
        return View(new MatAcceptanceViewModel { LocalReturnAddress = returnAddress });
    }

    [Authorize]
    [HttpPost(RouteConstants.Start.MatAcceptance)]
    public IActionResult MatAcceptanceApprove([FromQuery] string? returnAddress)
    {
        HttpContext.Session.SetString(SessionKeyConstants.MatAccepted, bool.TrueString);
        return Redirect(returnAddress ?? RouteConstants.Root);
    }
}
