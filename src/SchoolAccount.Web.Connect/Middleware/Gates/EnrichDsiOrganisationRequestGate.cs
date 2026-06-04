using SchoolAccount.Application.Extensions;
using SchoolAccount.Kernel;
using SchoolAccount.Kernel.Organisations;
using SchoolAccount.Web.Connect.Extensions;
using SchoolAccount.Web.Connect.Middleware.Interfaces;
using SchoolAccount.Web.Connect.Middleware.Models;

namespace SchoolAccount.Web.Connect.Middleware.Gates;

public class EnrichDsiOrganisationRequestGate(
    IUserContext userContext,
    IOrganisationContext organisationContext
) : IRequestGate
{
    public int Priority { get; } = -int.MaxValue;

    public async Task<bool> CanEvaluateAsync(HttpContext context)
    {
        return !(!userContext.IsAuthenticated
               || !await organisationContext.IsAuthorised()
               || !organisationContext.IsDsiDetermined
               || !string.IsNullOrEmpty(context.Session.GetString(SessionKeyConstants.OrgSelected))
               || context.Request.IsRestrictedPath(
                   RouteConstants.Start.PickAOrganisation.RemoveOptionalUrlProperties()));
    }

    public Task<GateResult> EvaluateAsync(HttpContext context)
    {
        var accepted = context.Session.GetString(SessionKeyConstants.CommunicatedWithAcademyApi);

        if (accepted == bool.TrueString)
        {
            return Task.FromResult(
                GateResult.Continue());
        }

        var url = RouteConstants.Start.PickAOrganisation + "?returnAddress={returnAddress}";
        return Task.FromResult(
            GateResult.Redirect(
                url.Format(new
                {
                    type = organisationContext.Organisation is TrustOrganisation ? "trust" : "establishment",
                    ukprn = organisationContext.Organisation.Ukrpn,
                    returnAddress = context.Request.Path.Value
                })));
    }
}