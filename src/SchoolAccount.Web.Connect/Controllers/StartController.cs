using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolAccount.Application.Extensions;
using SchoolAccount.Integration.AcademiesApi.Models;
using SchoolAccount.Integration.AcademiesApi.Services;
using SchoolAccount.Integration.DfESignIn.Models;
using SchoolAccount.Integration.DfESignIn.Services;
using SchoolAccount.Kernel;
using SchoolAccount.Kernel.Organisations;
using SchoolAccount.Web.Connect.Authentication.Attributes;
using SchoolAccount.Web.Connect.Authentication.Exceptions;
using SchoolAccount.Web.Connect.Models;
using SchoolAccount.Web.Connect.Models.Start;

namespace SchoolAccount.Web.Connect.Controllers;

public class StartController(
    IDsiApiService dsiApiService,
    IOrganisationApiService organisationApiService,
    ITrustApiService trustApiService,
    IUserContext userContext, 
    IOrganisationContext organisationContext,
    IHttpContextAccessor contextAccessor
) : Controller
{
    [AllowAnonymous]
    [HttpGet(RouteConstants.Start.Index)]
    public IActionResult Index([FromQuery] string? returnUrl)
    {
        return !userContext.IsAuthenticated
            ? View(new StartIntroductionViewModel(returnUrl))
            : LocalRedirect(RouteConstants.Root);
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
        return LocalRedirect(returnAddress ?? RouteConstants.Root);
    }

    [Authorize]
    [HttpGet(RouteConstants.Start.SelectAOrganisation)]
    public async Task<IActionResult> SelectAOrganisationAsync([FromQuery] string? returnAddress)
    {
        if (organisationContext.IsDsiDetermined)
        {
            throw new InterruptionException("You cannot manually select a organisation outside of DfE Sign In workflow");
        }
        
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
        
        var organisations = (await GetUsersScope())
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
        var context = contextAccessor.HttpContext;

        if (context is null)
        {
            return BadRequest();
        }
        
        var suffix = string.Empty;

        if (impersonate)
        {
            suffix = SessionKeyConstants.ImpersonateSuffix;
        }
        
        context.Session.Remove(SessionKeyConstants.ComputedOrg + suffix);

        switch (type)
        {
            case "academy":
            {
                var establishment = await organisationApiService.GetEstablishment(ukprn);

                if (establishment is null)
                {
                    throw new InterruptionException("Establishment not found");
                }

                var isTrust = context.Session.GetString(SessionKeyConstants.OrgType) == "trust";

                if (!impersonate && !isTrust && (await GetUsersScope()).All(x => x.UkPrn != establishment.Ukprn))
                {
                    throw new InterruptionException("Establishment not within your enrollments" );
                }

                if (isTrust && !string.IsNullOrEmpty(context.Session.GetString(SessionKeyConstants.OrgSelected)))
                {
                    var trust = JsonSerializer.Deserialize<AcademyTrust>(
                        context.Session.GetString(SessionKeyConstants.OrgSelected)!);

                    if (trust?.Establishments.Any(x => establishment.Ukprn == x.Ukprn) == false)
                    {
                        throw new InterruptionException("Establishment is not within your trusts scope");
                    }
                }

                context.Session.SetString(SessionKeyConstants.OrgSelected + suffix,
                    JsonSerializer.Serialize(establishment));
            }
                break;
            case "trust":
            {
                var trust = await trustApiService.GetTrust(ukprn);

                if (trust is null)
                {
                    throw new InterruptionException("Trust not found");
                }

                if ((await GetUsersScope()).All(x => x.UkPrn != trust.GiasData?.Ukprn))
                {
                    throw new InterruptionException("Trust not within your enrollments");
                }

                context.Session.SetString(SessionKeyConstants.OrgSelected + suffix, JsonSerializer.Serialize(trust));
            }
                break;
            default:
                throw new NotImplementedException(type);
        }
        
        context.Session.SetString(SessionKeyConstants.OrgType + suffix, type);
        context.Session.SetString(SessionKeyConstants.UkPrn + suffix, ukprn);
        contextAccessor.HttpContext!.Session.SetString(SessionKeyConstants.CommunicatedWithAcademyApi + suffix, ukprn);
        
        return LocalRedirect(returnAddress ?? RouteConstants.Root);
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

    private async Task<List<OrganisationClaim>> GetUsersScope()
    {
        if (string.IsNullOrEmpty(userContext.DsiIdentifier))
        {
            return [];
        }

        return await dsiApiService.GetUserOrganisations(userContext.DsiIdentifier);
    }
}
