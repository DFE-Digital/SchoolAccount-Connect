using Microsoft.FeatureManagement;
using SchoolAccount.Web.Connect.Middleware.Interfaces;
using SchoolAccount.Web.Connect.Middleware.Models;

namespace SchoolAccount.Web.Connect.Middleware.Gates;

public class MaintenanceRequestGate(IFeatureManager featureManager) : IRequestGate
{
    public int Priority { get; } = 1;

    public async Task<GateResult> EvaluateAsync(HttpContext context)
    {
        if (!(await featureManager.IsEnabledAsync(FeatureNameConstants.MaintenanceMode)))
        {
            return GateResult.Continue();
        }

        return GateResult.Redirect(RouteConstants.Maintenance);
    }
}