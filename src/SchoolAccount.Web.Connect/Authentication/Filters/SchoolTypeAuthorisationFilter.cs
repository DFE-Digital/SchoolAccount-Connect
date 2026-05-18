using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SchoolAccount.Kernel;

namespace SchoolAccount.Web.Connect.Authentication.Filters;

public class SchoolTypeAuthorisationFilter(IOrganisationContext organisationContext, SchoolType[] allowedTypes)
    : IAsyncAuthorizationFilter
{
    public Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        ArgumentNullException.ThrowIfNull(organisationContext);
        ArgumentNullException.ThrowIfNull(allowedTypes);
        ArgumentNullException.ThrowIfNull(context);

        if (!context.HttpContext.User.Identity?.IsAuthenticated ?? true)
        {
            return Task.CompletedTask;
        }

        if (!allowedTypes.Any(a => organisationContext.Type.ContainsValue(a)))
        {
            context.Result = new ForbidResult();
        }

        return Task.CompletedTask;
    }
}
