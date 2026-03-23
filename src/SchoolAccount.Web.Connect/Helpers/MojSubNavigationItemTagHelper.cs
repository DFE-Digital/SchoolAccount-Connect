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

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var a = new TagBuilder("a");
        a.AddCssClass("moj-sub-navigation__link");
        a.Attributes["href"] = Href;

        if (Current)
        {
            a.Attributes["aria-current"] = "page";
        }

        a.InnerHtml.AppendHtml(output.GetChildContentAsync().Result);

        var li = new TagBuilder("li");
        li.AddCssClass("moj-sub-navigation__item");
        li.InnerHtml.AppendHtml(a);

        output.TagName = null;
        output.Content.SetHtmlContent(li);
    }
}
