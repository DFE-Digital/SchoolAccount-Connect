using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolAccount.Web.Connect.Helpers;

namespace SchoolAccount.Web.Connect.Extensions;

internal static class HtmlHelperExtensions
{
    internal static IHtmlContent MojFrontendStyleImports(this IHtmlHelper htmlHelper)
    {
        return new HtmlString($"<link rel=\"stylesheet\" href=\"{PageTemplateHelper.GetMojStyleAssetUrl()}\" />");
    }

    internal static IHtmlContent DfeFrontendStyleImports(this IHtmlHelper htmlHelper)
    {
        return new HtmlString(
            $"<link rel=\"stylesheet\" href=\"{PageTemplateHelper.GetDfeStyleAssetUrl()}\" asp-append-version=\"true\" />"
        );
    }
}
