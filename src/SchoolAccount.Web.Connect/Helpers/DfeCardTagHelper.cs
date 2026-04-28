using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SchoolAccount.Web.Connect.Helpers;

[HtmlTargetElement("dfe-card")]
public class DfeCardTagHelper : TagHelper
{
    public required string Title { get; set; }
    public required string Link { get; set; }
    public required string Description { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = null; // remove original tag

        var encoder = HtmlEncoder.Default;
        var title = encoder.Encode(Title);
        var url = encoder.Encode(Link);
        var description = encoder.Encode(Description);

        var baseLinkClasses = encoder.Encode(
            "govuk-link govuk-link--no-visited-state dfe-card-link--header dfe-card-link--support support-guidance-regulations-link-ga"
        );

        var divCard = new TagBuilder("div");
        divCard.AddCssClass("dfe-card");

        var divContainer = new TagBuilder("div");
        divContainer.AddCssClass("dfe-card-container");

        var h3 = new TagBuilder("h3");
        h3.AddCssClass("govuk-heading-s");

        var a = new TagBuilder("a")
        {
            Attributes =
            {
                ["href"] = url,
                ["target"] = "_blank",
                ["rel"] = "noopener noreferrer",
            },
        };

        a.AddCssClass(baseLinkClasses);
        a.InnerHtml.AppendHtml(title);
        h3.InnerHtml.AppendHtml(a);

        var p = new TagBuilder("p");
        p.AddCssClass("card-list-content");

        p.InnerHtml.AppendHtml(description);

        divContainer.InnerHtml.AppendHtml(h3);
        divContainer.InnerHtml.AppendHtml(p);
        divCard.InnerHtml.AppendHtml(divContainer);

        output.Content.SetHtmlContent(divCard);

        await Task.CompletedTask;
    }
}
