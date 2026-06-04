using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolAccount.Application.Extensions;
using SchoolAccount.Integration.AcademiesApi.Services;
using SchoolAccount.Integration.DfESignIn.Services;
using SchoolAccount.Kernel;
using SchoolAccount.Kernel.Organisations;
using SchoolAccount.Web.Connect.Authentication.Attributes;
using SchoolAccount.Web.Connect.Models;
using SchoolAccount.Web.Connect.Models.Start;

namespace SchoolAccount.Web.Connect.Controllers;

public class StartController(
    IUserContext userContext, 
    IDsiApiService dsiApiService,
    IOrganisationApiService organisationApiService,
    ITrustApiService trustApiService,
    IHttpContextAccessor contextAccessor
) : Controller
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

    [Authorize]
    [HttpGet(RouteConstants.Start.SelectAOrganisation)]
    public async Task<IActionResult> SelectAOrganisationAsync([FromQuery] string? returnAddress)
    {
        if (string.IsNullOrEmpty(userContext.DsiIdentifier))
        {
            return View(new SelectAOrganisationViewModel { Message = "Could not determine your user's identifier" });
        }
        
        contextAccessor.HttpContext!.Session.Remove(SessionKeyConstants.OrgType);
        contextAccessor.HttpContext!.Session.Remove(SessionKeyConstants.OrgSelected);
        contextAccessor.HttpContext!.Session.Remove(SessionKeyConstants.CommunicatedWithAcademyApi);
        contextAccessor.HttpContext!.Session.Remove(SessionKeyConstants.OrgType + SessionKeyConstants.ImpersonateSuffix);
        contextAccessor.HttpContext!.Session.Remove(SessionKeyConstants.OrgSelected + SessionKeyConstants.ImpersonateSuffix);
        contextAccessor.HttpContext!.Session.Remove(SessionKeyConstants.CommunicatedWithAcademyApi + SessionKeyConstants.ImpersonateSuffix);
        
        var organisations = (await dsiApiService.GetUserOrganisations(userContext.DsiIdentifier))
            .Where(o => !string.IsNullOrEmpty(o.UkPrn) && o.Category is not null)
            .ToCollection();
        
        return View(new SelectAOrganisationViewModel
        {
            Organisations = organisations
        });
    }

    [Authorize]
    [HttpGet(RouteConstants.Start.PickAOrganisation)]
    public async Task<IActionResult> PickAsync([FromRoute] string type, [FromRoute] string ukprn,
        [FromQuery] string? returnAddress, bool impersonate = false)
    {
        var suffix = string.Empty;

        if (impersonate)
        {
            suffix = SessionKeyConstants.ImpersonateSuffix;
        }
        
        contextAccessor.HttpContext!.Session.Remove(SessionKeyConstants.ComputedOrg + suffix);
        contextAccessor.HttpContext!.Session.SetString(SessionKeyConstants.OrgType + suffix, type);
        contextAccessor.HttpContext!.Session.SetString(SessionKeyConstants.UkPrn + suffix, ukprn);

        switch (type)
        {
            case "academy":
                var organisation = await organisationApiService.GetEstablishment(ukprn);
                contextAccessor.HttpContext!.Session.SetString(SessionKeyConstants.OrgSelected + suffix, JsonSerializer.Serialize(organisation));
                break;
            case "trust":
                var trust = await trustApiService.GetTrust(ukprn);
                contextAccessor.HttpContext!.Session.SetString(SessionKeyConstants.OrgSelected + suffix, JsonSerializer.Serialize(trust));
                break;
            default:
                throw new NotImplementedException(type);
        }
        
        contextAccessor.HttpContext!.Session.SetString(SessionKeyConstants.CommunicatedWithAcademyApi + suffix, ukprn);
        
        return Redirect(returnAddress ?? RouteConstants.Root);
    }

    [Authorize]
    [HttpGet(RouteConstants.Start.ReturnToTrust)]
    public async Task<IActionResult> ReturnToTrustAsync([FromQuery] string? returnAddress)
    {
        contextAccessor.HttpContext!.Session.Remove(SessionKeyConstants.ComputedOrg +
                                                    SessionKeyConstants.ImpersonateSuffix);
        contextAccessor.HttpContext!.Session.Remove(SessionKeyConstants.OrgSelected +
                                                    SessionKeyConstants.ImpersonateSuffix);
        contextAccessor.HttpContext!.Session.Remove(SessionKeyConstants.OrgType +
                                                    SessionKeyConstants.ImpersonateSuffix);
        contextAccessor.HttpContext!.Session.Remove(SessionKeyConstants.CommunicatedWithAcademyApi +
                                                    SessionKeyConstants.ImpersonateSuffix);
        return LocalRedirect(returnAddress ?? RouteConstants.Root);
    }
}
