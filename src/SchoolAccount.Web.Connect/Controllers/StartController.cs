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
    public IActionResult Index()
    {
        return !userContext.IsAuthenticated ? View() : Redirect(RouteConstants.Root);
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
        [FromQuery] string? returnAddress)
    {
        contextAccessor.HttpContext!.Session.SetString(SessionKeyConstants.OrgType, type);
        contextAccessor.HttpContext!.Session.SetString(SessionKeyConstants.UkPrn, ukprn);
        
        switch (type)
        {
            case "academy":
                var organisation = await organisationApiService.GetEstablishment(ukprn);
                contextAccessor.HttpContext!.Session.SetString(SessionKeyConstants.OrgSelected, JsonSerializer.Serialize(organisation));
                //contextAccessor.HttpContext!.Session.Remove(SessionKeyConstants.SelectedTrustUkRpn);
                break;
            case "trust":
                var trust = await trustApiService.GetTrust(ukprn);
                contextAccessor.HttpContext!.Session.SetString(SessionKeyConstants.OrgSelected, JsonSerializer.Serialize(trust));
                contextAccessor.HttpContext!.Session.SetString(SessionKeyConstants.SelectedTrustUkRpn, ukprn);
                break;
            default:
                throw new NotImplementedException(type);
        }
        
        return Redirect(returnAddress ?? RouteConstants.Root);
    }

    [Authorize]
    [HttpGet(RouteConstants.Start.ReturnToTrust)]
    public async Task<IActionResult> ReturnToTrustAsync()
    {
        var trustUkRpn = contextAccessor.HttpContext!.Session.GetString(SessionKeyConstants.SelectedTrustUkRpn);
        return string.IsNullOrEmpty(trustUkRpn)
            ? throw new ArgumentException(trustUkRpn)
            : RedirectToAction("Pick", new { type = "trust", ukprn = trustUkRpn });
    }
}
