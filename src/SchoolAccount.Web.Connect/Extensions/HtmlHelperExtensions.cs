using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolAccount.Web.Connect.Helpers;
using SchoolAccount.Web.Connect.HtmlGeneration;

namespace SchoolAccount.Web.Connect.Extensions;

internal static class HtmlHelperExtensions
{
    internal static IHtmlContent MojFrontendScriptImports(this IHtmlHelper htmlHelper)
    {
        return HtmlContentExtensions.Combine(
            ComponentGenerator.Generate("script",
                attributes: [("type", "module"), ("src", PageTemplateHelper.GetMojScriptAssetUrl())]),
            ComponentGenerator.Generate("script", text: PageTemplateHelper.GetMojScriptInlineScript(),
                treatAsHtml: true, attributes: [("type", "module"), ("type", "text/javascript")])
        );
    }

    internal static IHtmlContent MojFrontendStyleImports(this IHtmlHelper htmlHelper)
    {
        return new HtmlString(
            $"<link rel=\"stylesheet\" href=\"{PageTemplateHelper.GetMojStyleAssetUrl()}\" />");
    }

    internal static IHtmlContent DfeFrontendStyleImports(this IHtmlHelper htmlHelper)
    {
        return new HtmlString(
            $"<link rel=\"stylesheet\" href=\"{PageTemplateHelper.GetDfeStyleAssetUrl()}\" asp-append-version=\"true\" />");
    }
}