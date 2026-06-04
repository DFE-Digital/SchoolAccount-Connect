using SchoolAccount.Kernel;
using SchoolAccount.Kernel.Organisations;
using SchoolAccount.Web.Connect.Middleware.Interfaces;
using SchoolAccount.Web.Connect.Middleware.Models;

namespace SchoolAccount.Web.Connect.Middleware.Gates;

public class SelectAOrganisationRequestGate(IUserContext userContext, IOrganisationContext organisationContext) : IRequestGate
{
    public int Priority { get; } = 5;

    public Task<bool> CanEvaluateAsync(HttpContext context)
    {
        var outcome = !userContext.IsAuthenticated
                      || organisationContext.IsDsiDetermined
                      || context.Request.Path.StartsWithSegments(
                          RouteConstants.Start.SelectAOrganisation,
                          StringComparison.InvariantCultureIgnoreCase
                      );
        
        return Task.FromResult(!outcome);
    }

    public Task<GateResult> EvaluateAsync(HttpContext context)
    {
        var accepted = context.Session.GetString(SessionKeyConstants.OrgType);

        if (!string.IsNullOrEmpty(accepted))
        {
            return Task.FromResult(GateResult.Continue());
        }

        var returnUrl = context.Request.Path + context.Request.QueryString;
        return Task.FromResult(
            GateResult.Redirect($"{RouteConstants.Start.SelectAOrganisation}?return={Uri.EscapeDataString(returnUrl)}")
        );
    }
}
