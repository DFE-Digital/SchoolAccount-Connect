using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SchoolAccount.Kernel;

namespace SchoolAccount.Web.Connect.Authentication.Filters;

public class OrganisationTypeAuthorisationFilter : IAsyncAuthorizationFilter
{
    private readonly IOrganisationContext _organisationContext;
    private readonly Type[] _allowedOrganisations;

    public OrganisationTypeAuthorisationFilter(IOrganisationContext organisationContext, Type[] allowedOrganisations)
    {
        if (!allowedOrganisations.All(x => x.IsAssignableTo(typeof(IOrganisation))))
        {
            throw new ArgumentException(
                $"All {nameof(allowedOrganisations)} types must implement {nameof(IOrganisation)}",
                nameof(allowedOrganisations)
            );
        }

        _organisationContext = organisationContext;
        _allowedOrganisations = allowedOrganisations;
    }

    public Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        ArgumentNullException.ThrowIfNull(_organisationContext);
        ArgumentNullException.ThrowIfNull(_allowedOrganisations);
        ArgumentNullException.ThrowIfNull(context);

        if (!context.HttpContext.User.Identity?.IsAuthenticated ?? true)
        {
            return Task.CompletedTask;
        }

        if (!_allowedOrganisations.Contains(_organisationContext.Organisation.GetType()))
        {
            context.Result = new ForbidResult();
        }

        return Task.CompletedTask;
    }
}
