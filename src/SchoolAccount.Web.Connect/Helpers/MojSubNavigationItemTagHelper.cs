using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SchoolAccount.Web.Connect.Helpers;

[HtmlTargetElement("moj-sub-navigation-item", ParentTag = "moj-sub-navigation")]
public class MojSubNavigationItemTagHelper : TagHelper
{
    [HtmlAttributeName("href")]
    public string Href { get; set; } = "#";

    [HtmlAttributeName("current")]
    public bool Current { get; set; } = false;

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var a = new TagBuilder("a");
        a.AddCssClass("moj-sub-navigation__link");
        a.Attributes["href"] = Href;
        a.Attributes["role"] = "tab";

        if (Current)
        {
            a.Attributes["aria-current"] = "true";
            a.Attributes["aria-selected"] = "true";
        }
        else
        {
            a.Attributes["aria-selected"] = "false";
        }

        a.InnerHtml.AppendHtml(await output.GetChildContentAsync());

        var li = new TagBuilder("li");
        li.AddCssClass("moj-sub-navigation__item");
        li.Attributes["role"] = "presentation";
        li.InnerHtml.AppendHtml(a);

        output.TagName = null;
        output.Content.SetHtmlContent(li);
    }
}
