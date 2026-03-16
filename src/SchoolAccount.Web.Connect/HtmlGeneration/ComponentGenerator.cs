using System.Text.Encodings.Web;
using GovUk.Frontend.AspNetCore;
using Microsoft.AspNetCore.Html;
using SoftCircuits.HtmlMonkey;
using ReadOnlyTagHelperAttributeList = Microsoft.AspNetCore.Razor.TagHelpers.ReadOnlyTagHelperAttributeList;
using TagHelperAttribute = Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute;
using TagHelperAttributeList = Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttributeList;
using TagHelperOutput = Microsoft.AspNetCore.Razor.TagHelpers.TagHelperOutput;

namespace SchoolAccount.Web.Connect.HtmlGeneration;

internal static partial class ComponentGenerator
{
    internal static void ApplyComponentHtml(
        this TagHelperOutput output,
        IHtmlContent content,
        HtmlEncoder? encoder = null
    )
    {
        ArgumentNullException.ThrowIfNull(output);
        ArgumentNullException.ThrowIfNull(content);

        encoder ??= HtmlEncoder.Default;
        ArgumentNullException.ThrowIfNull(encoder);

        var unwrapped = UnwrapComponent(content, encoder);
        ArgumentNullException.ThrowIfNull(unwrapped);

        output.TagName = unwrapped.TagName;
        output.TagMode = unwrapped.TagMode;

        output.Attributes.Clear();

        foreach (var attribute in unwrapped.Attributes)
        {
            output.Attributes.Add(attribute);
        }

        output.Content.AppendHtml(unwrapped.InnerHtml);
    }

    internal static ComponentTagHelperOutput? UnwrapComponent(IHtmlContent content, HtmlEncoder encoder)
    {
        return UnwrapComponent(content.ToHtmlString(encoder));
    }

    internal static ComponentTagHelperOutput? UnwrapComponent(string html)
    {
        if (string.IsNullOrWhiteSpace(html))
        {
            return null;
        }

        var doc = HtmlDocument.FromHtml(html);
        var root = (HtmlElementNode)doc.RootNodes.Single(n => n is HtmlElementNode);

        var tagName = root.TagName;
        var tagMode = root.IsSelfClosing
            ? Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing
            : Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag;
        var attributes = new TagHelperAttributeList(
            root.Attributes.Select(a =>
                a.Value is null
                    ? new TagHelperAttribute(a.Name)
                    : new TagHelperAttribute(a.Name, new HtmlString(a.Value))
            )
        );
        var innerHtml = new HtmlString(root.InnerHtml);

        return new ComponentTagHelperOutput(tagName, tagMode, attributes, innerHtml);
    }

    internal sealed record ComponentTagHelperOutput(
        string? TagName,
        Microsoft.AspNetCore.Razor.TagHelpers.TagMode TagMode,
        ReadOnlyTagHelperAttributeList Attributes,
        IHtmlContent InnerHtml
    );
}
