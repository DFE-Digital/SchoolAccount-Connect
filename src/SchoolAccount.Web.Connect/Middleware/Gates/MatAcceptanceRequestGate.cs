using SchoolAccount.Kernel;
using SchoolAccount.Kernel.Organisations;
using SchoolAccount.Web.Connect.Middleware.Interfaces;
using SchoolAccount.Web.Connect.Middleware.Models;

namespace SchoolAccount.Web.Connect.Middleware.Gates;

public class MatAcceptanceRequestGate(IUserContext userContext, IOrganisationContext organisationContext) : IRequestGate
{
    public int Priority { get; } = 2;

    public Task<GateResult> EvaluateAsync(HttpContext context)
    {
        if (
            !userContext.IsAuthenticated
            || organisationContext.Organisation is not TrustOrganisation
            || context.Request.Path.StartsWithSegments(
                RouteConstants.Start.MatAcceptance,
                StringComparison.InvariantCultureIgnoreCase
            )
        )
        {
            return Task.FromResult(GateResult.Continue());
        }

        var accepted = context.Session.GetString(SessionKeyConstants.MatAccepted);

        if (accepted == bool.TrueString)
        {
            return Task.FromResult(GateResult.Continue());
        }

        var returnUrl = context.Request.Path + context.Request.QueryString;
        return Task.FromResult(
            GateResult.Redirect($"{RouteConstants.Start.MatAcceptance}?return={Uri.EscapeDataString(returnUrl)}")
        );
    }
}
