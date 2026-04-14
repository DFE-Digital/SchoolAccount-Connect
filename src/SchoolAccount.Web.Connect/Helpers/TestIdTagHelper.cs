using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SchoolAccount.Web.Connect.Helpers;

[HtmlTargetElement("*", Attributes = TestIdTagName)]
public class TestIdTagHelper(IWebHostEnvironment env) : TagHelper
{
    private const string TestIdTagName = "test-id";
    
    [HtmlAttributeName(TestIdTagName)]
    public string? TestId { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.Attributes.RemoveAll(TestIdTagName);
        output.Attributes.RemoveAll("data-" + TestIdTagName);

        if (!env.IsProduction() && !string.IsNullOrEmpty(TestId))
        {
            output.Attributes.Add("data-" + TestIdTagName, TestId);
        }
    }
}