using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SchoolAccount.Web.Connect.Extensions;

public static class HtmlContentExtensions
{
    public static IHtmlContent Combine(params TagBuilder?[] tags)
    {
        var builder = new HtmlContentBuilder();

        foreach (var tag in tags)
        {
            if (tag != null)
            {
                builder.AppendHtml(tag);
            }
        }

        return builder;
    }

    public static string GenerateTestId(params string[] parts)
    {
        return string.Join(
            "-",
            parts.Select(x => x.Trim().Replace(" ", string.Empty, StringComparison.CurrentCultureIgnoreCase))
        );
    }
}
