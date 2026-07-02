using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolAccount.Kernel;
using SchoolAccount.Kernel.Organisations;
using SchoolAccount.Web.Connect.Authentication.Attributes;

namespace SchoolAccount.Web.Connect.Features.Start;

public class StartController(IUserContext userContext) : Controller
{
    [AllowAnonymous]
    [HttpGet(RouteConstants.Start.Index)]
    public IActionResult Index([FromQuery] string? returnUrl)
    {
        return !userContext.IsAuthenticated
            ? View(new StartIntroductionViewModel(returnUrl))
            : Redirect(RouteConstants.Root);
    }

    [HttpGet(RouteConstants.Start.MatAcceptance)]
    [RestrictOrganisationType(typeof(TrustOrganisation))]
    public IActionResult MatAcceptance([FromQuery] string? returnAddress)
    {
        return View(new MatAcceptanceViewModel { LocalReturnAddress = returnAddress });
    }

    [HttpPost(RouteConstants.Start.MatAcceptance)]
    public IActionResult MatAcceptanceApprove([FromQuery] string? returnAddress)
    {
        HttpContext.Session.SetString(SessionKeyConstants.MatAccepted, bool.TrueString);
        return Redirect(returnAddress ?? RouteConstants.Root);
    }
}
