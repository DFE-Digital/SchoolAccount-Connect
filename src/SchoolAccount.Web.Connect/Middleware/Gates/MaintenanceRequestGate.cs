using Microsoft.FeatureManagement;
using SchoolAccount.Web.Connect.Middleware.Interfaces;
using SchoolAccount.Web.Connect.Middleware.Models;
using static System.StringComparison;
using static SchoolAccount.Application.Constants.FeatureFlagConstants;
using static SchoolAccount.Web.Connect.RouteConstants;

namespace SchoolAccount.Web.Connect.Middleware.Gates;

public class MaintenanceRequestGate(IFeatureManager featureManager) : IRequestGate
{
    public int Priority { get; } = 1;

    public async Task<GateResult> EvaluateAsync(HttpContext context)
    {
        var maintenanceModeDisabled = !await featureManager.IsEnabledAsync(MaintenanceMode);

        if (maintenanceModeDisabled)
        {
            return GateResult.Continue();
        }

        var isOnMaintenancePage = context.Request.Path.StartsWithSegments(Maintenance, OrdinalIgnoreCase);

        if (isOnMaintenancePage)
        {
            return GateResult.Continue();
        }

        return GateResult.Redirect(Maintenance);
    }
}
