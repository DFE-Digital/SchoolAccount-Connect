using Markdig;
using Microsoft.AspNetCore.Html;

namespace SchoolAccount.Web.Connect.Extensions;

public static class MarkdigExtensions
{
    public static string? FromMarkdown(this string? markdown, MarkdownPipeline? pipeline = null)
    {
        pipeline ??= new MarkdownPipelineBuilder()
            .UseAdvancedExtensions()
            .Build();
        
        return !string.IsNullOrWhiteSpace(markdown) 
            ? Markdown.ToHtml(markdown, pipeline)
            : null;
    }
    
    public static HtmlString? ToHtml(this string? markdown)
    {
        return !string.IsNullOrWhiteSpace(markdown)
            ? new HtmlString(markdown)
            : null;
    }
}