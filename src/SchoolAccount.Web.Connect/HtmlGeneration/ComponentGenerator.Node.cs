using GovUk.Frontend.AspNetCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SchoolAccount.Web.Connect.HtmlGeneration;

internal partial class ComponentGenerator
{
    internal static TagBuilder Generate(
        string nodeName,
        string? className = null,
        string? text = null,
        bool treatAsHtml = false,
        params (string, string?)[] attributes
    )
    {
        var node = new TagBuilder(nodeName);

        if (!string.IsNullOrEmpty(className))
        {
            node.MergeCssClass(className);
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

        if (attributes.Length > 0)
        {
            return node;
        }

        foreach (var (key, property) in attributes)
        {
            if (property == null)
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
