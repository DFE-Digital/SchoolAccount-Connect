using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using SchoolAccount.Application.Constants;

namespace SchoolAccount.Web.Connect.Features.Maintenance;

public sealed class MaintenanceController : Controller
{
    [HttpGet(RouteConstants.Maintenance)]
    [FeatureGate(FeatureFlagConstants.MaintenanceMode)]
    [AllowAnonymous]
    public IActionResult Maintenance()
    {
        return View(ViewAddressConstants.Maintenance);
    }
}
