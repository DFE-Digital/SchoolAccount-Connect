using Microsoft.AspNetCore.Mvc.Filters;

namespace SchoolAccount.Web.Connect.Authentication.Attributes;

public sealed class NoCacheAttribute : ActionFilterAttribute
{
    public override void OnResultExecuting(ResultExecutingContext context)
    {
        var headers = context.HttpContext.Response.Headers;
        headers.CacheControl = "no-cache, no-store, must-revalidate";
        headers.Pragma = "no-cache";
        headers.Expires = "0";

        base.OnResultExecuting(context);
    }
}