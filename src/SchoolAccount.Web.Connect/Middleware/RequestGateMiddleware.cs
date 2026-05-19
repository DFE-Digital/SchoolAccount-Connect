using SchoolAccount.Web.Connect.Middleware.Interfaces;
using static Microsoft.AspNetCore.Http.HttpResults.RedirectHttpResult;

namespace SchoolAccount.Web.Connect.Middleware;

public class RequestGateMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, IEnumerable<IRequestGate> gates)
    {
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

            if (IsLocalUrl(result.RedirectAddress))
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
