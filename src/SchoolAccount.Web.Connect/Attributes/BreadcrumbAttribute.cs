namespace SchoolAccount.Web.Connect.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public sealed class BreadcrumbAttribute(string title, string? url = null, int order = 0) : Attribute
{
    public string Title { get; } = title;
    public string? Url { get; } = url;
    public int Order { get; } = order;
}
