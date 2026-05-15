using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SchoolAccount.Web.Connect.Attributes;
using SchoolAccount.Web.Connect.Models.Breadcrumb;

namespace SchoolAccount.Web.Connect.Filters;

public class BreadcrumbActionFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context) { }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Result is not ViewResult viewResult)
            return;

        var attrs = context
            .ActionDescriptor.EndpointMetadata.OfType<BreadcrumbAttribute>()
            .OrderBy(b => b.Order)
            .ToList();

        if (attrs.Count == 0)
            return;

        var attrCrumbs = attrs.Select(a => new Breadcrumb { Title = a.Title, Url = a.Url }).ToList();

        // If the action added dynamic crumbs, prepend the attribute crumbs before them
        if (viewResult.ViewData["Breadcrumbs"] is List<Breadcrumb> existing)
        {
            attrCrumbs.AddRange(existing);
        }

        viewResult.ViewData["Breadcrumbs"] = attrCrumbs;
    }
}
