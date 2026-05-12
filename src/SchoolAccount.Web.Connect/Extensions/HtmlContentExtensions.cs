namespace SchoolAccount.Web.Connect.Extensions;

public static class HtmlContentExtensions
{
    public static string GenerateTestId(params string[] parts)
    {
        return string.Join(
            "-",
            parts.Select(x => x.Trim().Replace(" ", string.Empty, StringComparison.CurrentCultureIgnoreCase))
        );
    }
}
