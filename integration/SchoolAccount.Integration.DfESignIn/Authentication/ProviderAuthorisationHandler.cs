using Microsoft.AspNetCore.Authorization;
using SchoolAccount.Integration.DfESignIn.Exceptions;
using SchoolAccount.Integration.DfESignIn.Extensions;
using SchoolAccount.Integration.DfESignIn.Interfaces;
using SchoolAccount.Integration.DfESignIn.Providers;
using SchoolAccount.Integration.DfESignIn.Requirements;

namespace SchoolAccount.Integration.DfESignIn.Authentication;

public class ProviderAuthorisationHandler(IProviderContext providerContext) : AuthorizationHandler<ProviderRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ProviderRequirement requirement
    )
    {
        if (!context.User.Identity?.IsAuthenticated ?? true)
        {
            return;
        }

        var provider = providerContext.Provider;

        if (provider == NullProvider.Default)
        {
            throw new NoProviderException();
        }

        if (!requirement.IsProviderAllowed(provider))
        {
            return;
        }

        if (!await provider.CanAccess(context.User.GetOrganisation()))
        {
            throw new ProviderAuthorisationException();
        }

        context.Succeed(requirement);
    }
}
