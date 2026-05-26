using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SchoolAccount.Integration.DfESignIn.Extensions;
using SchoolAccount.Integration.DfESignIn.Interfaces;

namespace SchoolAccount.Integration.DfESignIn.Filters;

public class ProviderAuthorisationFilter(IProviderContext organisationContext, Type[] allowedProviders)
    : IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        if (context.HttpContext.User.Identity?.IsAuthenticated != true)
        {
            return;
        }

        var provider = organisationContext.Provider;

        if (!provider.IsProviderAllowed(allowedProviders))
        {
            context.Result = new ForbidResult();
            return;
        }

        if (!await provider.CanAccess(context.HttpContext.GetOrganisation()))
        {
            context.Result = new ForbidResult();
        }
    }
}
