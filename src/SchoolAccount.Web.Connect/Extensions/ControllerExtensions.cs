using Microsoft.AspNetCore.Mvc;
using SchoolAccount.Web.Connect.Models.Breadcrumb;

namespace SchoolAccount.Web.Connect.Extensions;

public static class ControllerExtensions
{
    public static void AddBreadcrumb(this Controller controller, string? title, string? url = null)
    {
        if (title is null)
        {
            return;
        }

        var breadcrumbs = controller.ViewData["Breadcrumbs"] as List<Breadcrumb> ?? [];

        breadcrumbs.Add(new Breadcrumb { Title = title, Url = url });

        controller.ViewData["Breadcrumbs"] = breadcrumbs;
    }
}
