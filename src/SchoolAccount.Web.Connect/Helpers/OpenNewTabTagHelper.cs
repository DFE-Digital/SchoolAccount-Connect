using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SchoolAccount.Web.Connect.Helpers;

[HtmlTargetElement("a", Attributes = OpenNewTabTagName)]
public class OpenNewTabTagHelper : TagHelper
{
    private const string OpenNewTabTagName = "open-new-tab";

    public override int Order => int.MinValue;

    [HtmlAttributeName(OpenNewTabTagName)]
    public bool? OpenInNewTab { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.Attributes.RemoveAll(OpenNewTabTagName);

        if (OpenInNewTab.HasValue && OpenInNewTab.Value)
        {
            output.Attributes.Add("target", "_blank");
            output.Attributes.Add("rel", "noreferrer noopener");

            var content = await output.GetChildContentAsync();
            output.Content.SetHtmlContent($"{content.GetContent()} (opens in new tab)");
        }
    }
}
