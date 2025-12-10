using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolAccount.Web.Manage.Helpers;
using SchoolAccount.Web.Manage.HtmlGeneration;

namespace SchoolAccount.Web.Manage.Extensions;

public static class HtmlHelperExtensions
{
    public static IHtmlContent MojFrontendScriptImports(this IHtmlHelper htmlHelper)
    {
        return HtmlContentExtensions.Combine(
            ComponentGenerator.Generate("script",
                attributes: [("type", "module"), ("src", PageTemplateHelper.GetMojScriptAssetUrl())]),
            ComponentGenerator.Generate("script", text: PageTemplateHelper.GetMojScriptInlineScript(),
                treatAsHtml: true, attributes: [("type", "module"), ("type", "text/javascript")])
        );
    }

    public static IHtmlContent MojFrontendStyleImports(this IHtmlHelper htmlHelper)
    {
        return new HtmlString(
            $"<link rel=\"stylesheet\" href=\"{PageTemplateHelper.GetMojStyleAssetUrl()}\" />");
    }

    public static IHtmlContent DfeFrontendStyleImports(this IHtmlHelper htmlHelper)
    {
        return ComponentGenerator.Generate("link",
            attributes:
            [
                ("rel", "stylesheet"), ("href", PageTemplateHelper.GetDfeStyleAssetUrl()),
                ("asp-append-version", "true")
            ]);
    }
}