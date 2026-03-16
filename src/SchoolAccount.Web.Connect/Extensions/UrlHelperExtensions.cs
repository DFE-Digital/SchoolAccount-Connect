namespace SchoolAccount.Web.Connect.Extensions;

public static class UrlHelperExtensions
{
    public static bool IsLocalUrl(this string? url)
    {
        if (string.IsNullOrEmpty(url))
        {
            return false;
        }

        if (url[0] == '/')
        {
            if (url.Length == 1)
            {
                return true;
            }

            if (url[1] != '/' && url[1] != '\\')
            {
                return true;
            }

            return false;
        }

        if (url[0] == '~' && url.Length > 1 && url[1] == '/')
        {
            if (url.Length == 2)
            {
                return true;
            }

            if (url[2] != '/' && url[2] != '\\')
            {
                return true;
            }
        }

        return false;
    }

    public static bool IsLocalUrl(this Uri url)
    {
        return url.ToString().IsLocalUrl();
    }
}
