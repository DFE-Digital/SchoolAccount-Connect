using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using SchoolAccount.Web.Connect.Middleware.Interfaces;

namespace SchoolAccount.Web.Connect.Middleware;

public class RequestGateMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, IUrlHelperFactory urlHelperFactory,
        IActionContextAccessor actionContextAccessor, IEnumerable<IRequestGate> gates)
    {
        var urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext!);
        
        foreach (var gate in gates.OrderBy(x => x.Priority))
        {
            var result = await gate.EvaluateAsync(context);

            if (!result.ShouldRedirect)
            {
                continue;
            }

            if (result.RedirectAddress is null)
            {
                throw new NullReferenceException(nameof(result.RedirectAddress));
            }

            if (urlHelper.IsLocalUrl(result.RedirectAddress))
            {
                context.Response.Redirect(result.RedirectAddress);
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
            }

            return;
        }

        await next(context);
    }
}