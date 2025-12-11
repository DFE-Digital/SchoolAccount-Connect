using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SchoolAccount.Web.Manage.Extensions;

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
}