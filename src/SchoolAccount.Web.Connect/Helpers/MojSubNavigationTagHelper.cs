using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SchoolAccount.Web.Connect.Helpers;

[HtmlTargetElement("moj-sub-navigation")]
public class MojSubNavigationTagHelper : TagHelper
{
    [HtmlAttributeName("aria-label")]
    public string AriaLabel { get; set; } = "Sub navigation";
    
    [HtmlAttributeName("show-accessibility-label")]
    public bool ShowAccessibleActiveLabel { get; set; } = false;

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var childContent = await output.GetChildContentAsync();

        output.TagName = "nav";
        output.Attributes.SetAttribute("class", "moj-sub-navigation");
        output.Attributes.SetAttribute("aria-label", AriaLabel);

        var ul = new TagBuilder("ul");
        ul.AddCssClass("moj-sub-navigation__list");
        ul.Attributes["role"] = "tablist";
        ul.InnerHtml.AppendHtml(childContent);

        output.Content.SetHtmlContent(ul);
    }
}
