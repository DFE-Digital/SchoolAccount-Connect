using Microsoft.AspNetCore.Mvc.Rendering;

namespace SchoolAccount.Web.Connect.HtmlGeneration;

internal partial class ComponentGenerator
{
    internal static TagBuilder Generate(
        string nodeName,
        string? className = null,
        string? text = null,
        bool treatAsHtml = false,
        params (string, string?)[] attributes)
    {
        var node = new TagBuilder(nodeName);

        if (!string.IsNullOrWhiteSpace(className))
        {
            foreach (var cssClass in className.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            {
                node.AddCssClass(cssClass);
            }
        }

        if (!string.IsNullOrEmpty(text))
        {
            if (treatAsHtml)
            {
                node.InnerHtml.AppendHtml(text);
            }
            else
            {
                node.InnerHtml.Append(text);
            }
        }

        foreach (var (key, property) in attributes)
        {
            if (property is null)
            {
                node.Attributes.Remove(key);
            }
            else
            {
                node.MergeAttribute(key, property);
            }
        }

        return node;
    }
}