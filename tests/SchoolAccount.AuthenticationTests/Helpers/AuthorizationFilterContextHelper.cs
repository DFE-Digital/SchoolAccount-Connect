using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace SchoolAccount.AuthenticationTests.Helpers;

public static class AuthorizationFilterContextHelper
{
    public static AuthorizationFilterContext CreateContext(bool authenticated)
    {
        var httpContext = new DefaultHttpContext();

        if (authenticated)
        {
            httpContext.User = ClaimsPrincipalHelper.CreateUser();
        }

        var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
        return new AuthorizationFilterContext(actionContext, []);
    }
}
